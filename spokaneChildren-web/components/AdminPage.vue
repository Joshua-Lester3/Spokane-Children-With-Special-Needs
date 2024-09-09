<template>
  <v-container>
    <v-card>
      <v-sheet color="blue">
        <v-row no-gutters>
          <v-col class="me-auto" cols="auto">
            <v-card-title>
              Admin Utilities
            </v-card-title>
          </v-col>
          <v-col cols="auto">
            <v-btn icon="mdi-plus" color="blue" elevation="0" rounded="0"
              @click="router.push({ path: '/userEdit', query: { id: 'new' } })" />
          </v-col>
        </v-row>
      </v-sheet>
      <v-list>
        <v-list-subheader>Users</v-list-subheader>
        <template v-for="user in userList">
          <div v-if="user.userName !== tokenService.getUserName()">
            <v-divider />
            <v-list-item :title="user.userName ?? ''" :subtitle="user.email ?? ''">
              <template v-slot:append>
                <v-list-item-subtitle class="mr-4">
                  {{ user.role }}
                </v-list-item-subtitle>
                <v-list-item-action>
                  <v-btn icon="mdi-pencil" elevation="0"
                    @click="router.push({ path: '/userEdit', query: { id: user.userId } })" />
                </v-list-item-action>
              </template>
            </v-list-item>
          </div>
        </template>
      </v-list>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import Axios from 'axios';
import TokenService from '~/scripts/tokenService';
import type User from '~/scripts/user';

const userList = ref<Array<User>>([]);
const router = useRouter();
const tokenService = new TokenService();

onMounted(async () => {
  try {
    const url = 'user/getUserList';
    const response = await Axios.get(url);
    userList.value = response.data;
  } catch (error) {
    console.error('Error fetching user list: ', error)
  }
});

</script>