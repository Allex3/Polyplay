<script setup lang="ts">
import MaliciousActivitiesLog from './MaliciousActivitiesLog.vue'
import UserActivitiesLog from './UserActivitiesLog.vue'
</script>

<template>
  <div class="flex flex-col gap-10">
    <UserActivitiesLog />
    <MaliciousActivitiesLog />
  </div>
</template>
