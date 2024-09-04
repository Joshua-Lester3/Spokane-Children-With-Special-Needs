<template>
  <v-alert v-model="success" tile title="Success" type="success" closable>
    Successfully posted Event!
  </v-alert>
  <v-btn icon="mdi-chevron-left" elevation="0" class="mt-5 ml-2" @click="router.push('/')" />
  <v-container>
    <v-card class="mt-3">
      <div class="ma-4">
        <v-row>
          <v-col cols="4">
            <p class="mt-4">Event name:</p>
          </v-col>
          <v-col>
            <v-text-field v-model="eventName" label="Event Name" />
          </v-col>
        </v-row>
        <v-date-picker v-model="date" />
        <v-row no-gutters>
          <v-col>
            <v-select :items="hours" label="Hour" class="mr-2" v-model="hoursModel" />
          </v-col>
          <v-col>
            <v-select :items="minutes" label="Minutes" class="mr-2" v-model="minutesModel" />
          </v-col>
          <v-col>
            <v-select :items="amPm" label="AM/PM" v-model="amPmModel" />
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="4">
            <p class="mt-4">Location: </p>
          </v-col>
          <v-col>
            <v-text-field v-model="location" label="Location" />
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="5">
            <p class="mt-4">Description: </p>
          </v-col>
          <v-col>
            <v-textarea v-model="description" label="Description" />
          </v-col>
        </v-row>
        <v-row>
          <v-col cols="4">
            <p class="mt-4">Link (optional):</p>
          </v-col>
          <v-col>
            <v-text-field v-model="link" label="Link" />
          </v-col>
        </v-row>
      </div>
      <v-card-actions>
        <v-spacer />
        <v-btn v-if="!isCreating" variant="tonal" prepend-icon="mdi-delete" class="ma-2" color="error"
          @click="deleteDialog = true">Delete</v-btn>
        <v-btn variant="elevated" class="ma-2" color="success" @click="postEvent">Submit</v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
  <delete-entity-dialog v-model="deleteDialog" type="Event" @accept="deleteEvent" />
</template>

<script setup lang="ts">
import Axios from 'axios';
import type Event from '~/scripts/event';
import { useDate } from 'vuetify';
import TokenService from '~/scripts/tokenService';


const router = useRouter();
const tokenService = new TokenService();
let eventId: number;
const route = useRoute();
const eventName = ref<string>('');
const location = ref<string>('');
const description = ref<string>('');
const link = ref<string | null>('');
const isCreating = ref(false);
const deleteDialog = ref(false);
const success = ref(false);
const date = ref(new Date());
date.value.setSeconds(0, 0);
const hoursModel = ref<string>('12');
const minutesModel = ref<string>('30');
const amPmModel = ref<string>('PM');

watch([date, hoursModel, minutesModel, amPmModel], () => {
  updateDate();
});

function updateDate() {
  let hoursParsed = parseInt(hoursModel.value);
  let minutesParsed = parseInt(minutesModel.value);
  if (amPmModel.value === 'PM') {
    if (hoursParsed !== 12) {
      hoursParsed += 12;
    }
  } else {
    if (hoursParsed === 12) {
      hoursParsed = 0;
    }
  }
  date.value.setHours(hoursParsed);
  date.value.setMinutes(minutesParsed);
  console.log(date.value);
}


onMounted(async () => {
  updateDate();
  let stringId = route.query.id as string;
  eventId = parseInt(stringId);
  console.log(eventId);
  if (eventId !== -1) {
    try {
      const url = `event/getEvent?id=${eventId}`;
      const response = await Axios.get(url);
      let event: Event = response.data;
      eventName.value = event.eventName;
      date.value = new Date(Date.parse(event.dateTime));
      date.value.setHours(date.value.getHours() - 7);
      hoursModel.value = `${convertDbHours(date.value.getHours())}`;
      minutesModel.value = `${convertDbMinutes(date.value.getMinutes())}`;
      amPmModel.value = date.value.getHours() > 11 ? 'PM' : 'AM';
      location.value = event.location;
      description.value = event.description;
      link.value = event.link;
    } catch (error) {
      console.error('Error fetching selected event:', error);
    }
  } else {
    isCreating.value = true;
  }
});

async function postEvent() {
  try {
    const headers = tokenService.generateTokenHeader();
    success.value = false;
    const url = 'event/addEvent';
    const response = await Axios.post(url, {
      eventId: isCreating.value ? -1 : eventId,
      eventName: eventName.value,
      description: description.value,
      dateTime: date.value.toISOString(),
      location: location.value,
      link: link.value,
    }, { headers });
    eventId = response.data.eventId;
    success.value = true;
  } catch (error) {
    console.log('Error posting event: ', error);
  }
}

async function deleteEvent() {
  try {
    const headers = tokenService.generateTokenHeader();
    const url = `event/deleteEvent/${eventId}`;
    await Axios.post(url, null, { headers });
    router.push({ path: '/', query: { page: 0 } });
  } catch (error) {
    console.log('Error deleting event: ', error);
  }
}

const hours = [
  '1',
  '2',
  '3',
  '4',
  '5',
  '6',
  '7',
  '8',
  '9',
  '10',
  '11',
  '12',
];

const minutes = [
  '00',
  '05',
  '10',
  '15',
  '20',
  '25',
  '30',
  '35',
  '40',
  '45',
  '50',
  '55',
];

const amPm = [
  'AM',
  'PM',
];

function convertDbHours(hours: number): number {
  if (hours === 0) {
    return 1;
  } else if (hours > 12) {
    return hours % 12;
  } else if (hours === 12) {
    return 12;
  } else {
    return hours;
  }
}

function convertDbMinutes(minutes: number) {
  if (minutes === 0) {
    return '00';
  }
  if (minutes === 5) {
    return '05'
  }
  return minutes;
}

</script>