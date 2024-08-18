<template>
  <v-app>
    <v-app-bar :elevation="2">
      <template v-slot:prepend>
        <v-app-bar-nav-icon @click.prevent.stop="showNavDrawer = true" />
      </template>

      <v-app-bar-title>Spokane Children with Special Needs</v-app-bar-title>
    </v-app-bar>

    <ClientOnly>
      <v-navigation-drawer v-model="showNavDrawer" :width="navigationDrawerWidth" disable-resize-watcher temporary>
        <v-list class="text-center">
          <v-list-item>
            Hello
          </v-list-item>
        </v-list>
      </v-navigation-drawer>
    </ClientOnly>

    <v-main>
      <NuxtPage />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { useDisplay } from 'vuetify';

const display = ref(useDisplay());

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

</script>