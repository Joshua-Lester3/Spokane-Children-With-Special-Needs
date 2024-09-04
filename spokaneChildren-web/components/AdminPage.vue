<template>
  <v-container>
    <v-card>
      <v-sheet color="blue">
        <v-card-title>
          Admin Utilities
        </v-card-title>
      </v-sheet>
      <v-list>
        <v-list-subheader>Users</v-list-subheader>
        <v-list-item v-for="user in userList" :title="user.userName ?? ''" :subtitle="user.email ?? ''">
          <template v-slot:append>
            <v-list-item-action>
              <v-btn icon="mdi-pencil" elevation="0" />
            </v-list-item-action>
          </template>
        </v-list-item>
      </v-list>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import Axios from 'axios';
import type User from '~/scripts/user';

const userList = ref<Array<User>>([]);

onMounted(async () => {
  try {
    const url = 'user/getUserList';
    const response = await Axios.get(url);
    userList.value = response.data;
  } catch (error) {
    console.error('Error fetching user list: ', error)
  }
});

</script>