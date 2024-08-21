<template>
  <v-dialog v-model="modelValue">
    <v-card min-height="200" class="mx-auto" min-width="300">
      <v-sheet>
        <v-card-title>Deletion</v-card-title>
      </v-sheet>
      <v-card-text>
        Would you like to delete this announcement?
        <v-card>
          <v-card-title>
            {{ announcement.title }}
          </v-card-title>
          <v-card-subtitle>
            {{ announcement.datePosted }}
            <br />
            {{ announcement.description }}
          </v-card-subtitle>
        </v-card>
      </v-card-text>
      <v-card-actions>
        <v-btn variant="text" @click="modelValue = false">Cancel</v-btn>
        <v-btn color="error" variant="tonal" @click="emits('accept'); modelValue = false;">Delete</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import type Announcement from '~/scripts/announcement';

const modelValue = defineModel<boolean>();
const props = defineProps<{
  announcement: Announcement
}>();
const emits = defineEmits(['accept']);
</script>