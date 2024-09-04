<template>
  <v-tabs v-model="tab" align-tabs="start" color="blue">
    <v-tab :value="0" @click="router.push({ path: '/', query: { page: 0 } })">Home</v-tab>
    <v-tab :value="1" @click="router.push({ path: '/', query: { page: 1 } })">Resources</v-tab>
    <v-tab v-if="tokenService.isAdmin()" :value="2" @click="router.push({ path: '/', query: { page: 2 } })">Admin</v-tab>
  </v-tabs>
  <v-tabs-window v-model="tab">
    <v-tabs-window-item :value="0">
      <HomePage />
    </v-tabs-window-item>
    <v-tabs-window-item :value="1">
      <ResourcesPage />
    </v-tabs-window-item>
    <v-tabs-window-item v-if="tokenService.isAdmin()" :value="2">
      <AdminPage />
    </v-tabs-window-item>
  </v-tabs-window>
</template>

<script setup lang="ts">
import TokenService from '~/scripts/tokenService';

const tab = ref();
const route = useRoute();
const router = useRouter();
const tokenService = new TokenService();

onMounted(() => {
  let stringId = route.query.page as string | undefined;
  if (stringId !== undefined) {
    tab.value = parseInt(stringId);
    console.log(tab.value);
  } else {
    tab.value = 0;
  }
});
</script>