<template>
  <v-container>
    <v-card class="ma-3">
      <v-card-title>
        {{ announcement?.title }}
      </v-card-title>
      <v-card-subtitle>
        {{ announcement?.datePosted }}
      </v-card-subtitle>
      <p class="ma-4">
        {{ announcementDescriptionText }}
      </p>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import Axios from 'axios';
import type Announcement from '~/scripts/announcement';

let announcementId: number;
const route = useRoute();
const announcement = ref<Announcement | undefined>(); // will be set in mounted hook
const announcementDescriptionText = computed(() => {
  if (announcement.value?.description === null || announcement.value?.description.length === 0) {
    return '(No content.)';
  } else {
    announcement.value?.description;
  }
})

onMounted(async () => {
  try {
    let stringId = route.query.id as string;
    announcementId = parseInt(stringId);
    console.log(announcementId);
    const url = `announcement/getAnnouncement?id=${announcementId}`;
    const response = await Axios.get(url);
    announcement.value = response.data;
  } catch (error) {
    console.error('Error fetching selected announcement:', error);
  }
});

</script>