<template>
  <v-alert v-model="success" tile title="Success" type="success" closable>
    Successfully posted Event!
  </v-alert>
  <v-btn icon="mdi-chevron-left" elevation="0" class="mt-5 ml-2" @click="router.push('/')" />
  <v-container>
    <v-card class="mt-3">
      <v-container>
        <v-text-field v-model="name" label="Name" />
        <v-text-field v-model="website" label="Website" />
        <v-text-field v-model="phone" label="Phone" />
        <v-text-field v-model="address" label="Address" />
        <v-select v-model="category" label="Category" :items="categories" />
      </v-container>
      <v-card-actions>
        <v-spacer />
        <v-btn v-if="!isCreating" variant="tonal" prepend-icon="mdi-delete" class="ma-2" color="error"
          @click="deleteDialog = true">Delete</v-btn>
        <v-btn variant="elevated" class="ma-2" color="success" @click="postResource">Submit</v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
  <delete-entity-dialog v-model="deleteDialog" type="Event" @accept="deleteResource" />
</template>

<script setup lang="ts">
import Axios from 'axios';
import { type Resource, ResourceCategory } from '~/scripts/resource';

const route = useRoute();
const router = useRouter();
let resourceId: number;
const name = ref('');
const website = ref<string | null>('');
const phone = ref<string | null>('');
const address = ref<string | null>('');
const category = ref<string>('Therapy');
const isCreating = ref(false);
const deleteDialog = ref(false);
const success = ref(false);

onMounted(async () => {
  let stringId = route.query.id as string;
  resourceId = parseInt(stringId);
  console.log(resourceId);
  if (resourceId !== -1) {
    try {
      const url = `resource/getResource?id=${resourceId}`;
      const response = await Axios.get(url);
      let resource: Resource = response.data;
      name.value = resource.name;
      website.value = resource.website;
      phone.value = resource.phone;
      address.value = resource.address;
      category.value = categoryToString(resource.category);
    } catch (error) {
      console.error('Error fetching selected resource:', error);
    }
  } else {
    isCreating.value = true;
  }
});

async function postResource() {
  try {
    const url = 'resource/addResource';
    await Axios.post(url, {
      resourceId: resourceId,
      name: name.value,
      category: stringToCategory(category.value),
      website: website.value,
      phone: phone.value,
      address: address.value,
    });
    success.value = true;
  } catch (error) {
    console.log('Error posting resource: ', error);
  }
}

async function deleteResource() {
  try {
    const url = `resource/deleteResource/${resourceId}`;
    await Axios.post(url, null);
    router.push('/resources');
  } catch (error) {
    console.log('Error deleting resource: ', error);
  }
}

const categories = [
  'Therapy',
  'Psychiatrist',
  'Other',
];

function stringToCategory(category: string) {
  switch (category) {
    case 'Therapy':
      return ResourceCategory.Therapy;
    case 'Psychiatrist':
      return ResourceCategory.Psychiatrist;
    default:
      return ResourceCategory.Other;
  }
}

function categoryToString(category: ResourceCategory) {
  switch (category) {
    case ResourceCategory.Therapy:
      return 'Therapy';
    case ResourceCategory.Psychiatrist:
      return 'Psychiatrist';
    default:
      return 'Other';
  }
}
</script>