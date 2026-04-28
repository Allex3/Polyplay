<script setup lang="ts">
import { Suspense } from 'vue'
import EditGameComponent from './EditGameComponent.vue'
</script>
<template>
  <Suspense>
    <EditGameComponent />
    <template #fallback>Loading...</template>
  </Suspense>
</template>
