<script setup lang="ts">
import { Suspense } from 'vue'
import GamesOnTags from './GamesOnTags.vue'
</script>

<template>
  <Suspense>
    <GamesOnTags />
    <template class="text-center w-full h-full" #fallback>Loading...</template>
  </Suspense>
</template>
