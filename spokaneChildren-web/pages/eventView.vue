<template>
  <v-btn icon="mdi-chevron-left" elevation="0" class="mt-5 ml-2"
    @click="    router.push({ path: '/', query: { page: 0 } })" />
  <v-container>
    <v-card class="ma-3">
      <v-card-title>
        {{ event?.eventName }}
      </v-card-title>
      <v-card-subtitle>
        {{ `Date: ${event?.dateTime}` }}
        <br />
        {{ `Location: ${event?.location}` }}
        <br />
        Link:
        <NuxtLink v-if="event?.link != null" :to="event?.link">{{ event?.link }}</NuxtLink>
      </v-card-subtitle>
      <div class="ma-4">
        {{ event?.description }}
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
    if (event.value != undefined) {
      let date = new Date(Date.parse(event.value.dateTime));
      date.setHours(date.getHours() - 7);
      event.value.dateTime = date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
    }
  } catch (error) {
    console.error('Error fetching selected event:', error);
  }
});

</script>