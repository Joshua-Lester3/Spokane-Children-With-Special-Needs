<template>
  <v-app>
    <v-app-bar :elevation="2" @click="showNavDrawer = false">
      <v-app-bar-title class="cursor-pointer" @click="router.push({ path: '/', query: { page: 0 } })">Spokane Children With
        Special
        Needs</v-app-bar-title>
    </v-app-bar>
    <v-main>
      <NuxtPage />
    </v-main>
    <v-footer class="bg-grey-lighten-1">
      <v-row justify="center" no-gutters>
        <v-btn v-for="link in links" :key="link.text" class="mx-2" color="white" rounded="xl" variant="text"
          @click="router.push(link.url)">{{ link.text }}
        </v-btn>
        <v-col class="text-center mt-4" cols="12">
          {{ new Date().getFullYear() }} — <strong>Title</strong>
        </v-col>
      </v-row>
    </v-footer>
  </v-app>
</template>

<script setup lang="ts">
const router = useRouter();
const showNavDrawer = ref(false);
const links = [
  {
    text: 'Home',
    url: '/',
  },
  {
    text: 'About',
    url: '/about',
  },
  {
    text: 'Login',
    url: '/login',
  }
];

</script>