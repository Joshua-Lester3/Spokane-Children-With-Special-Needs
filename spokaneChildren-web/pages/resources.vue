<template>
    <v-card class="mx-auto" max-width="750">
        <v-list v-model:opened="open">
            <v-list-group v-for="list in resourceLists">
                <template v-slot:activator="{ props }">
                    <v-list-item v-bind="props" :title="getEnumValue(list[0].category)"></v-list-item>
                    <v-divider />
                </template>
                <template v-for="(resource, i) in list" :key="i">
                    <v-list-item>
                        <v-list-item-title>
                            {{ resource.name }}
                            <template v-if="resource.website != null">
                                (
                                    <NuxtLink :to="resource.website">{{ resource.website }}</NuxtLink>
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

enum ResourceCategory {
    Therapy,
    Psychiatrist,
}

interface Resource {
    resourceId: number;
    category: ResourceCategory;
    name: string;
    website: string | null;
    phone: string | null;
    address: string | null;
}

const open = ref(['Admin']);
const resourceLists = ref<Array<Array<Resource>>>([]);

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

function getResourceName(resource: Resource) {
    let result = resource.name;
    if (resource.website != null) {
        result += ` ()`
    }
}

function getResourceContactInfo(resource: Resource) : string {
    let result = '';
    if (resource.website != null) {
        result += resource.website + ' | ';
    }
    if (resource.phone != null) {
        result += resource.phone + ' | ';
    }
    if (resource.address != null) {
        result += resource.address + ' | ';
    }
    return result;
}
</script>