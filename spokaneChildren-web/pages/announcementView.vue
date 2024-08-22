<template>
  <v-btn icon="mdi-chevron-left" elevation="0" class="mt-5 ml-2" @click="router.push('/')" />
  <v-container>
    <v-card class="ma-3">
      <v-card-title>
        {{ announcement?.title }}
      </v-card-title>
      <v-card-subtitle>
        {{ announcement?.datePosted }}
      </v-card-subtitle>
      <div class="ma-4">
        {{ announcementDescriptionText }}
      </div>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import Axios from 'axios';
import type Announcement from '~/scripts/announcement';

const router = useRouter();
let announcementId: number;
const route = useRoute();
const announcement = ref<Announcement | undefined>(); // will be set in mounted hook
const announcementDescriptionText = computed(() => {
  if (announcement.value?.description === null || announcement.value?.description.length === 0) {
    return '(No content.)';
  } else {
    return announcement.value?.description;
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