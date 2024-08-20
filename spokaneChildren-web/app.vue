<template>
  <v-app>
    <v-app-bar :elevation="2" @click="showNavDrawer = false">
      <template v-slot:prepend>
        <v-app-bar-nav-icon @click.prevent.stop="showNavDrawer = true" />
      </template>

      <v-app-bar-title>Title</v-app-bar-title>
    </v-app-bar>

    <ClientOnly>
      <v-navigation-drawer v-model="showNavDrawer" :width="navigationDrawerWidth" disable-resize-watcher temporary>
        <v-list class="text-center">
          <v-list-item @click="router.push('/'); showNavDrawer = false">Home</v-list-item>
          <v-list-item @click="router.push('/resources'); showNavDrawer = false">Resources</v-list-item>
        </v-list>
      </v-navigation-drawer>
    </ClientOnly>

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
import { useDisplay } from 'vuetify';
import TokenService from '~/scripts/tokenService';

const tokenService = ref(new TokenService());
provide('TOKEN', tokenService);
const display = ref(useDisplay());
const router = useRouter();
const showNavDrawer = ref(false);
const navigationDrawerWidth = computed(() => {
  switch (display.value.name) {
    case 'xs': return 175
    case 'sm': return 200;
    case 'md': return 225;
    case 'lg': return 250;
    case 'xl': return 275;
    case 'xxl': return 300;
  }
});
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