<template>
    <v-container>
        <v-row>
            <v-col sm="12" md="6">

                <v-card class="mx-auto" elevation="4">
                    <v-sheet color="blue">
                        <v-card-title>Announcements</v-card-title>
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
                        <v-card-title>
                            Events
                        </v-card-title>
                    </v-sheet>
                    <v-container>
                        <v-infinite-scroll mode="manual" @load="loadEvents">
                            <template v-for="event in events" :key="event.eventId">
                                <v-card class="mx-2 mb-5" height="175" width="auto" color="blue">
                                    <v-card-title>
                                        {{ event.eventName }}
                                    </v-card-title>
                                    <v-card-subtitle>
                                        {{ event.dateTime }}
                                    </v-card-subtitle>
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

interface Announcement {
    id: number;
    title: string;
    description: string | null;
    datePosted: string;
}

interface Event {
    eventId: number;
    eventName: string;
    description: string;
    dateTime: string;
    location: string;
    link: string | null;
}

const announcements = ref<Array<Announcement>>([]);
const announcementPageNumber = ref(0);
const events = ref<Array<Event>>([]);
const eventPageNumber = ref(0);

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

</script>