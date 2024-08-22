<template>
  <v-alert v-model="success" tile title="Success" type="success" closable>
    Successfully posted Announcement!
  </v-alert>
  <v-btn icon="mdi-chevron-left" elevation="0" class="mt-5 ml-2" @click="router.push('/')" />
  <v-container>
    <v-card class="ma-3">
      <v-card-title>
        <v-text-field v-model="title" label="Title" />
      </v-card-title>
      <v-textarea class="ma-4" v-model="description" label="Description" />
      <v-card-actions>
        <v-spacer />
        <v-btn v-if="!isCreating" variant="tonal" prepend-icon="mdi-delete" class="ma-2" color="error"
          @click="deleteDialog = true">Delete</v-btn>
        <v-btn variant="elevated" class="ma-2" color="success" @click="postAnnouncement">Submit</v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
  <delete-entity-dialog v-model="deleteDialog" type="Announcement" @accept="deleteAnnouncement" />
</template>

<script setup lang="ts">
import Axios from 'axios';
import type Announcement from '~/scripts/announcement';

const router = useRouter();
let announcementId: number;
const route = useRoute();
const title = ref<string>('');
const description = ref<string | null>('');
const success = ref(false);
const deleteDialog = ref(false);
const isCreating = ref(false);

onMounted(async () => {
  let stringId = route.query.id as string;
  announcementId = parseInt(stringId);
  console.log(announcementId);
  if (announcementId !== -1) { // !== -1 means (as far as we know) it's an existing announcement
    try {
      const url = `announcement/getAnnouncement?id=${announcementId}`;
      const response = await Axios.get(url);
      let announcement: Announcement = response.data;
      title.value = announcement.title;
      description.value = announcement.description;
    } catch (error) {
      console.error('Error fetching selected announcement:', error);
    }
  } else {
    isCreating.value = true;
  }
});

async function postAnnouncement() {
  try {
    const url = 'announcement/addAnnouncement';
    await Axios.post(url, {
      id: isCreating.value ? -1 : announcementId,
      title: title.value,
      description: description.value,
    });
    success.value = true;
  } catch (error) {
    console.log('Error posting announcement: ', error);
  }
}

async function deleteAnnouncement() {
  try {
    const url = `announcement/deleteAnnouncement/${announcementId}`;
    await Axios.post(url, null);
    router.push('/');
  } catch (error) {
    console.log('Error posting announcement: ', error);
  }
}
</script>