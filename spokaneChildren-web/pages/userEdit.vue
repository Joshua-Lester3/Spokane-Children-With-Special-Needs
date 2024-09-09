<template>
  <v-alert v-model="success" tile title="Success" type="success" closable>
    Successfully posted User!
  </v-alert>
  <v-alert v-model="errorAlert" tile title="Error" type="error" closable>
    <p v-for="errorMessage in errorMessages">
      {{ errorMessage }}
    </p>
  </v-alert>
  <v-btn icon="mdi-chevron-left" elevation="0" class="mt-5 ml-2"
    @click="router.push({ path: '/', query: { page: 2 } })" />
  <v-container>
    <v-card class="ma-3">
      <v-card-title>
        <v-text-field v-model="username" label="Username" />
      </v-card-title>
      <v-card-text>
        <v-text-field v-model="email" label="Email" />
        <v-select :items="roles" v-model="role" label="Role" />
        <v-text-field v-if="isCreating" v-model="password" label="Password" />
        <v-btn v-if="!isCreating" color="info" variant="elevated" @click="passwordDialog = true">Change password</v-btn>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn v-if="!isCreating" variant="tonal" prepend-icon="mdi-delete" class="ma-2" color="error"
          @click="deleteDialog = true">Delete</v-btn>
        <v-btn variant="elevated" class="ma-2" color="success" @click="postUser">Submit</v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
  <change-password-dialog v-if="!isCreating" v-model="passwordDialog" :id="userId" />
  <delete-entity-dialog v-model="deleteDialog" type="Announcement" @accept="deleteUser" />
</template>


<script setup lang="ts">
import Axios from 'axios';
import TokenService from '~/scripts/tokenService';
import type User from '~/scripts/user';

const route = useRoute();
let userId: string = '';
const email = ref<string | null>('');
const username = ref<string | null>('');
const role = ref<string | null>('');
let user: User;
const password = ref('');
const isCreating = ref(false);
const success = ref(false);
const errorAlert = ref(false);
const errorMessages = ref<Array<string>>([]);
const router = useRouter();
const deleteDialog = ref(false);
const passwordDialog = ref(false);
const tokenService = new TokenService();

onMounted(async () => {
  userId = route.query.id as string;
  console.log(userId);
  if (userId !== 'new') {
    try {
      const url = `user/getUser?id=${userId}`;
      const headers = tokenService.generateTokenHeader();
      const response = await Axios.get(url, { headers });
      user = response.data;
      email.value = user.email;
      username.value = user.userName;
      role.value = user.role;
    } catch (error) {
      console.error('Error fetching selected user:', error);
    }
  } else {
    isCreating.value = true;
  }
});

async function postUser() {
  success.value = false;
  errorAlert.value = false;
  const headers = tokenService.generateTokenHeader();
  if (isCreating.value) {
    try {
      const url = 'user/addUser';
      await Axios.post(url, {
        username: username.value,
        email: email.value,
        password: password.value,
        role: role.value
      }, { headers });
      success.value = true;
    } catch (error: any) {
      errorAlert.value = true;
      let errors = error.response.data.errors;
      errorMessages.value = [];
      if (errors === undefined) {
        errorMessages.value = [error.response.data];
      } else {
        for (let index = 0; index < errors.length; index++) {
          errorMessages.value = errorMessages.value.concat(errors[index].description);
        }
      }
      console.error('Error posting user data: ', error);
    }
  } else {
    try {
      const url = 'user/updateUserInfo';
      await Axios.post(url, {
        id: user.userId,
        newUsername: username.value,
        newEmail: email.value,
        newRole: role.value,
      }, { headers });
      success.value = true;
    } catch (error) {
      console.error('Error updating user info: ', error);
    }
  }
}

async function deleteUser() {
  const headers = tokenService.generateTokenHeader();
  try {
    const url = `user/deleteUser/${user.userId}`;
    await Axios.post(url, null, { headers });
    router.push({ path: '/', query: { page: 2 } });
  } catch (error) {
    console.error('Error deleting user: ', error);
  }
}

const roles = [
  'Admin',
  'Moderator',
  'None',
]
</script>