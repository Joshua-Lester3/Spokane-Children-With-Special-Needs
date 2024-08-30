<template>
  <v-container>
    <v-row>
      <v-col cols="12" md="6">
        <v-card class="mx-auto" elevation="4">
          <v-sheet color="blue">
            <v-row no-gutters>
              <v-col class="me-auto" cols="auto">
                <v-card-title>
                  Announcements
                </v-card-title>
              </v-col>
              <v-col cols="auto" v-if="isAdmin">
                <v-btn icon="mdi-plus" color="blue" elevation="0" rounded="0"
                  @click="router.push('/announcementEdit?id=-1')" />
              </v-col>
            </v-row>
          </v-sheet>
          <v-infinite-scroll mode="manual" @load="loadAnnouncements" empty-text="No more announcements">
            <template v-for="announcement in announcements" :key="announcement.id">
              <v-list-item class="py-2" @click="router.push(`/announcementView?id=${announcement.id}`)">
                <v-list-item-title>
                  {{ announcement.title }}
                </v-list-item-title>
                <v-list-item-subtitle>
                  {{ announcement.datePosted }}
                </v-list-item-subtitle>
                <template v-slot:append>
                  <p>{{ announcement.description }}</p>
                  <v-list-item-action v-if="isAdmin">
                    <v-btn class="ml-2" icon="mdi-pencil" elevation="0"
                      @click.stop="router.push(`/announcementEdit?id=${announcement.id}`)" />
                  </v-list-item-action>
                </template>
              </v-list-item>
              <v-divider />
            </template>
          </v-infinite-scroll>
        </v-card>
      </v-col>
      <v-col cols="12" md="6">
        <v-card class="mx-auto" elevation="4" width="auto">
          <v-sheet color="blue">
            <v-row no-gutters>
              <v-col class="me-auto" cols="auto">
                <v-card-title>
                  Events
                </v-card-title>
              </v-col>
              <v-col cols="auto" v-if="isAdmin">
                <v-btn icon="mdi-plus" color="blue" elevation="0" rounded="0"
                  @click="router.push('/eventEdit?id=-1')" />
              </v-col>
            </v-row>
          </v-sheet>
          <v-container>
            <v-infinite-scroll mode="manual" @load="loadEvents" empty-text="No more events">
              <template v-for="event in events" :key="event.eventId">
                <v-card class="mx-2 mb-5" height="175" width="auto" color="blue"
                  @click="router.push(`/eventView?id=${event.eventId}`)">
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
                        <v-btn icon="mdi-pencil" elevation="0"
                          @click.stop="router.push(`/eventEdit?id=${event.eventId}`)" />
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
</template>

<script setup lang="ts">
import Axios from 'axios';
import TokenService from '~/scripts/tokenService';
import type Announcement from '~/scripts/announcement';
import type Event from '~/scripts/event';

const router = useRouter();
const announcements = ref<Array<Announcement>>([]);
const announcementPageNumber = ref(0);
const events = ref<Array<Event>>([]);
const eventPageNumber = ref(0);
const tokenService = new TokenService();
const isAdmin = computed(() => tokenService.isAdmin());

await loadAnnouncements({ done: (message: string) => { } });
await loadEvents({ done: (message: string) => { } });

async function loadAnnouncements({ done }: { done: any }) {
  try {
    const url = `announcement/getAnnouncementList?page=${announcementPageNumber.value}`;
    const response = await Axios.get(url);
    response.data.forEach((element: Announcement) => {
      let date = new Date(Date.parse(element.datePosted));
      date.setHours(date.getHours() - 7);
      element.datePosted = date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
    });
    announcements.value = announcements.value.concat(response.data);
    announcementPageNumber.value = announcementPageNumber.value + 1;
    if (response.data.length > 0) {
      done('ok');
    } else {
      done('empty');
    }
  } catch (error) {
    console.error('Error fetching announcement list: ', error);
    done('error');
  }
}

async function loadEvents({ done }: { done: any }) {
  try {
    const url = `event/getEventList?page=${eventPageNumber.value}`;
    const response = await Axios.get(url);
    response.data.forEach((element: Event) => {
      let date = new Date(Date.parse(element.dateTime));
      date.setHours(date.getHours() - 7);
      element.dateTime = date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
    });
    events.value = events.value.concat(response.data);
    eventPageNumber.value = eventPageNumber.value + 1;
    if (response.data.length > 0) {
      done('ok');
    } else {
      done('empty');
    }

  } catch (error) {
    console.error('Error fetching event list: ', error);
    done('error');
  }
}
</script>