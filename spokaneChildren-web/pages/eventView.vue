<template>
  <v-btn icon="mdi-chevron-left" elevation="0" class="mt-5 ml-2" @click="router.push('/')" />
  <v-container>
    <v-card class="ma-3">
      <v-card-title>
        {{ event?.eventName }}
      </v-card-title>
      <v-card-subtitle>
        {{ `Date: ${event?.dateTime}` }}
        <br />
        {{ `Location: ${event?.location}` }}
      </v-card-subtitle>
      <div class="ma-4">
        {{ event?.description }}
        {{ event?.link }}
      </div>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import Axios from 'axios';
import type Event from '~/scripts/event';

const router = useRouter();
let eventId: number;
const route = useRoute();
const event = ref<Event | undefined>(); // will be set in mounted hook

onMounted(async () => {
  try {
    let stringId = route.query.id as string;
    eventId = parseInt(stringId);
    console.log(eventId);
    const url = `event/getEvent?id=${eventId}`;
    const response = await Axios.get(url);
    event.value = response.data;
  } catch (error) {
    console.error('Error fetching selected event:', error);
  }
});

</script>