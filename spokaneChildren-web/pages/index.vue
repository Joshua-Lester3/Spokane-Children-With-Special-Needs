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
                <v-card class="d-flex mx-auto" elevation="4" width="auto">
                    <v-container>
                        <v-card class="mx-auto mb-5" height="175" width="auto" color="blue" v-for="index in 2" :key="index">
                            {{ index }}
                        </v-card>
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
    const url = `announcement/getAnnouncementList?page=${announcementPageNumber.value}`;
    const response = Axios.get(url)
        .then((response) => {
            announcements.value = announcements.value.concat(response.data);
            announcementPageNumber.value = announcementPageNumber.value + 1;
        }).catch(error => {
            console.error('Error fetching announcement list: ', error);
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

</script>