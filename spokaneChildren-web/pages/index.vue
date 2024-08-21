<template>
  <v-container>
    <v-row>
      <v-col sm="12" md="6">
        <v-card class="mx-auto" elevation="4">
          <v-sheet color="blue">
            <v-row no-gutters>
              <v-col class="me-auto" cols="auto">
                <v-card-title>
                  Announcements
                </v-card-title>
              </v-col>
              <v-col cols="auto" v-if="isAdmin">
                <v-btn icon="mdi-plus" color="blue" elevation="0" rounded="0" />
              </v-col>
            </v-row>
          </v-sheet>
          <v-infinite-scroll mode="manual" @load="loadAnnouncements">
            <template v-for="announcement in announcements" :key="announcement.id">
              <v-list-item class="my-1">
                <v-list-item-title>
                  {{ announcement.title }}
                </v-list-item-title>
                <v-list-item-subtitle>
                  {{ announcement.datePosted }}
                </v-list-item-subtitle>
                <template v-slot:append>
                  <p>{{ announcement.description }}</p>
                  <v-list-item-action v-if="isAdmin">
                    <v-btn icon="mdi-pencil" elevation="0" />
                    <v-btn icon="mdi-delete" elevation="0"
                      @click="currentAnnouncement = announcement; announcementDialog = true;" />
                  </v-list-item-action>
                </template>
              </v-list-item>
              <v-divider />
            </template>
          </v-infinite-scroll>
        </v-card>
      </v-col>
      <v-col sm="12" md="6">
        <v-card class="mx-auto" elevation="4" width="auto">
          <v-sheet color="blue">
            <v-row no-gutters>
              <v-col class="me-auto" cols="auto">
                <v-card-title>
                  Events
                </v-card-title>
              </v-col>
              <v-col cols="auto" v-if="isAdmin">
                <v-btn icon="mdi-plus" color="blue" elevation="0" rounded="0" />
              </v-col>
            </v-row>
          </v-sheet>
          <v-container>
            <v-infinite-scroll mode="manual" @load="loadEvents">
              <template v-for="event in events" :key="event.eventId">
                <v-card class="mx-2 mb-5" height="175" width="auto" color="blue">
                  <v-row no-gutters>
                    <v-col class="me-auto" cols="auto">
                      <v-card-title>
                        {{ event.eventName }}
                      </v-card-title>
                      <v-card-subtitle>
                        {{ event.dateTime }}
                      </v-card-subtitle>

                    </v-col>
                    <v-col cols="auto" v-if="isAdmin">
                      <v-card-actions>
                        <v-btn icon="mdi-pencil" elevation="0" />
                        <v-btn icon="mdi-delete" elevation="0" />

                      </v-card-actions>
                    </v-col>
                  </v-row>
                </v-card>
              </template>
            </v-infinite-scroll>
          </v-container>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
  <delete-announcement-dialog v-model="announcementDialog" :announcement="currentAnnouncement"
    @accept="deleteAnnouncement" />
</template>

<script setup lang="ts">
import Axios from 'axios';
import TokenService from '~/scripts/tokenService';
import type Announcement from '~/scripts/announcement';


interface Event {
  eventId: number;
  eventName: string;
  description: string;
  dateTime: string;
  location: string;
  link: string | null;
}

const announcementDialog = ref<boolean>(false);
const currentAnnouncement = ref<Announcement>(null!); // will be set before announcementDialog is set to true
const announcements = ref<Array<Announcement>>([]);
const announcementPageNumber = ref(0);
const events = ref<Array<Event>>([]);
const eventPageNumber = ref(0);
const tokenService: Ref<TokenService> | undefined = inject('TOKEN');
const isAdmin = computed(() => tokenService?.value.isAdmin());

onMounted(async () => {
  const announcementUrl = `announcement/getAnnouncementList?page=${announcementPageNumber.value}`;
  Axios.get(announcementUrl)
    .then((response) => {
      announcements.value = announcements.value.concat(response.data);
      announcementPageNumber.value = announcementPageNumber.value + 1;
    }).catch(error => {
      console.error('Error fetching announcement list: ', error);
    });

  const eventUrl = `event/getEventList?page=${eventPageNumber.value}`;
  Axios.get(eventUrl)
    .then((response) => {
      events.value = events.value.concat(response.data);
      eventPageNumber.value = eventPageNumber.value + 1;
    }).catch(error => {
      console.error('Error fetching event list: ', error);
    });
});

function loadAnnouncements({ done }: { done: any }) {
  const url = `announcement/getAnnouncementList?page=${announcementPageNumber.value}`;
  Axios.get(url)
    .then((response) => {
      announcements.value = announcements.value.concat(response.data);
      announcementPageNumber.value = announcementPageNumber.value + 1;
      if (response.data.length > 0) {
        done('ok');
      } else {
        done('empty');
      }
    }).catch(error => {
      console.error('Error fetching announcement list: ', error);
      done('error');
    });
}

function loadEvents({ done }: { done: any }) {
  const url = `event/getEventList?page=${eventPageNumber.value}`;
  Axios.get(url)
    .then((response) => {
      debugger;
      events.value = events.value.concat(response.data);
      eventPageNumber.value = eventPageNumber.value + 1;
      if (response.data.length > 0) {
        done('ok');
      } else {
        done('empty');
      }
    }).catch(error => {
      console.error('Error fetching event list: ', error);
      done('error');
    });
}

function reloadAnnouncements() {
  announcements.value = [];
  let pages = announcementPageNumber.value;
  announcementPageNumber.value = 0;
  for (let i = 0; i < pages; i++) {
    loadAnnouncements({ done: (message: string) => { } });
  }
}

function deleteAnnouncement() {
  const url = `announcement/deleteAnnouncement/${currentAnnouncement.value.id}`
  Axios.post(url, null)
    .then((response) => {
      reloadAnnouncements();
    })
    .catch(error => {
      console.error('Error deleting announcement: ', error);
    });
}

</script>