<template>
  <v-dialog v-model="modelValue">
    <v-card min-height="200" class="mx-auto" min-width="300">
      <v-alert v-model="errorAlert" tile title="Error" type="error" closable>
        <p v-for="errorMessage in errorMessages">
          {{ errorMessage }}
        </p>
      </v-alert>
      <v-sheet color="blue">
        <v-card-title>Change Password</v-card-title>
      </v-sheet>
      <v-card-text>
        <v-text-field v-model="currentPassword" label="Current Password" />
        <v-text-field v-model="newPassword" label="New Password" />
      </v-card-text>
      <v-card-actions>
        <v-btn variant="text" @click="modelValue = false">Cancel</v-btn>
        <v-btn color="blue" variant="elevated" @click="changePassword()">Delete</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import Axios from 'axios';

const modelValue = defineModel<boolean>();
const currentPassword = ref('');
const newPassword = ref('');
const props = defineProps<{
  id: string
}>();
const errorAlert = ref(false);
const errorMessages = ref<Array<string>>([]);

async function changePassword() {
  try {
    const url = 'user/changePassword';
    await Axios.post(url, {
      id: props.id,
      currentPassword: currentPassword.value,
      newPassword: newPassword.value,
    });
    modelValue.value = false;
  } catch (error: any) {
    errorAlert.value = true;
    let errors = error.response.data.errors;
    errorMessages.value = [];
    if (errors === undefined) {
      errorMessages.value = [error.response.data];
    } else {
      for (let index = 0; index < errors.length; index++) {
        errorMessages.value = errorMessages.value.concat(errors[index].description);
      }
    }
    console.error('Error changing password: ', error);
  }
}
</script>