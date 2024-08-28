<template>
  <v-container class="align-center d-flex justify-center mt-2">
    <v-btn @click="router.push('/resourceEdit?id=-1')">Add Resource</v-btn>
  </v-container>
  <v-card class="mx-auto my-2" max-width="400">
    <v-list v-model:opened="open">
      <v-list-group v-for="list in resourceLists">
        <template v-slot:activator="{ props }">
          <v-list-item v-bind="props" :title="getEnumValue(list[0].category)"></v-list-item>
          <v-divider />
        </template>
        <template v-for="(resource, i) in list" :key="i">
          <v-list-item>
            <template v-if="isAdmin" v-slot:append>
              <v-btn class="ml-2" icon="mdi-pencil" elevation="0"
                @click.stop="router.push(`/resourceEdit?id=${resource.resourceId}`)" />
            </template>
            <v-list-item-title>
              {{ resource.name }}
            </v-list-item-title>
            <v-list-item-title>
              <template v-if="resource.website != null">
                (
                <NuxtLink :to="getResourceWebsite(resource.website)">{{ resource.website }}</NuxtLink>
                )
              </template>
            </v-list-item-title>
            <v-list-item-subtitle v-if="resource.phone != null">
              {{ resource.phone }}
            </v-list-item-subtitle>
            <v-list-item-subtitle v-if="resource.address != null">
              {{ resource.address }}
            </v-list-item-subtitle>
          </v-list-item>
          <v-divider />
        </template>
      </v-list-group>
    </v-list>
  </v-card>
</template>

<script setup lang="ts">
import Axios from 'axios';
import { type Resource, ResourceCategory } from '~/scripts/resource';
import TokenService from '~/scripts/tokenService';

const router = useRouter();
const open = ref(['Admin']);
const resourceLists = ref<Array<Array<Resource>>>([]);
const tokenService = new TokenService();
const isAdmin = computed(() => tokenService.isAdmin());

onMounted(async () => {
  try {
    const url = `resource/getResourceList`;
    const response = await Axios.get(url);
    resourceLists.value = response.data;
  } catch (error) {
    console.error('Error fetching resource list: ', error);
  }
});

function getEnumValue(category: ResourceCategory) {
  switch (category) {
    case ResourceCategory.Therapy:
      return 'Therapy';
    case ResourceCategory.Psychiatrist:
      return 'Psychiatrist';
    default:
      return 'Other';
  }
}

function getResourceWebsite(website: string): string {
  if (website.startsWith('https://')) {
    return website;
  }
  return 'https://' + website;
}
</script>