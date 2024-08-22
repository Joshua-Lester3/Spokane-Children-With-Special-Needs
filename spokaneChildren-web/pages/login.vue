<template>
  <v-progress-linear v-if="isBusy" color="grey" indeterminate />
  <v-alert v-model="hasError" tile icon="$warning" color="warning" title="Invalid input" :text="errorMessage"
    closable />
  <v-card elevation="3" height="auto" class="mx-auto my-10" width="auto" max-width="500">
    <v-card-text>
      <v-text-field v-model="username" label="Username" />
      <v-text-field v-model="password" label="Password" />
    </v-card-text>
    <v-card-actions>
      <v-spacer />
      <v-btn v-if="isLoggedIn" class="ma-3" @click="signOut">Sign out</v-btn>
      <v-btn v-else class="ma-3" variant="flat" color="secondary" @click="submit">Login</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script setup lang="ts">
import Axios from 'axios';
import TokenService from '~/scripts/tokenService';

const router = useRouter();
const username = ref<string>('');
const password = ref<string>('');
const tokenService = new TokenService();
const hasError = ref(false);
const errorMessage = ref('');
const isBusy = ref(false);
const isLoggedIn = computed(() => {
  isBusy.value;
  return tokenService.isLoggedIn();
});

async function submit() {
  const url = 'token/getToken';
  isBusy.value = true;
  await Axios.post(url, {
    username: username.value,
    password: password.value,
  })
    .then(async (response) => {
      tokenService.setToken(response.data.token);
      tokenService.setGuid(undefined);
      router.push('/');
    })
    .catch(error => {
      console.error('Error logging in.');
      hasError.value = true;
      errorMessage.value = error.response.data;
    });
  isBusy.value = false;
}

function signOut() {
  isBusy.value = true;
  tokenService.setToken('');
  tokenService.setGuid(null);
  isBusy.value = false;
}
</script>