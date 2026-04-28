<script setup lang="ts">
import { Suspense } from 'vue'
import UserGamesTable from './UserGamesTable.vue'
</script>
<template>
  <Suspense>
    <UserGamesTable />
    <template #fallback>Loading...</template>
  </Suspense>
</template>
