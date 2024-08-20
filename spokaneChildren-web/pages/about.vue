<template>
    <v-container>
        <v-card elevation="3" height="auto" class="my-2 mx-auto" width="auto" max-width="1000">
            <v-sheet class="bg-surface-light">
                <v-card-title class="font-weight-black">
                    About
                </v-card-title>
                <v-card-subtitle class="pb-2">
                    Subtitle
                </v-card-subtitle>

            </v-sheet>
            <v-card-text class=" pt-4">
                Content
            </v-card-text>
        </v-card>
    </v-container>
</template>

<script setup lang="ts">
const router = useRouter();
</script>