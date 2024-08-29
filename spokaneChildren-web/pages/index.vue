<template>
  <v-tabs v-model="tab" align-tabs="start" color="blue">
    <v-tab :value="0">Home</v-tab>
    <v-tab :value="1">Resources</v-tab>
  </v-tabs>
  <v-tabs-window v-model="tab">
    <v-tabs-window-item :value="0">
      <HomePage />
    </v-tabs-window-item>
    <v-tabs-window-item :value="0">
      <ResourcesPage />
    </v-tabs-window-item>
  </v-tabs-window>
</template>

<script setup lang="ts">

const tab = ref();
const route = useRoute();

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