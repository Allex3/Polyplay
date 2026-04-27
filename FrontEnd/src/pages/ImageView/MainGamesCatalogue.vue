<script setup lang="ts">
import { Suspense } from 'vue'
import GamesGrid from './GamesGrid.vue'
import SearchArt from './SearchArt.vue'
</script>
<template>
  <SearchArt />
  <Suspense>
    <GamesGrid />
    <template #fallback>Loading...</template>
  </Suspense>
</template>
