<script setup lang="ts">
import apiService from '@/api/apiService'
import { useShowProfileAndHideLogin } from '@/composables/useShowProfileAndHideLogin'
import { useUserRoles } from '@/composables/useUserRoles'
import type { MaliciousActivity } from '@/data/MaliciousActivity'
import { type UserActivity } from '@/data/UserActivity'
import { useUserStore } from '@/stores/userStore'
import { onBeforeMount, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const { isUserLoggedIn } = useShowProfileAndHideLogin()

const maliciousActivitiesLog = ref<MaliciousActivity[]>(
  await apiService.maliciousActivity.getMaliciousActivities(),
)

const { isUserAdmin } = useUserRoles()

onBeforeMount(() => {
  if (!isUserLoggedIn.value || !isUserAdmin) {
    router.push('/login')
  }
})
</script>
<template>
  <div
    class="retro-window gap-2 m-auto w-2/3 h-120 p-5 mb-5 flex flex-col overflow-x-scroll overflow-y-scroll"
  >
    <div class="flex flex-row gap-22 justify-center">
      <div>Id</div>
      <div>Username</div>
      <div>Activity Type</div>
      <div class="ml-10">Info</div>
      <div>IP</div>
    </div>
    <div v-for="maliciousAcitivity in maliciousActivitiesLog">
      <div class="flex flex-row gap-22 comic-neue items-center w-full text-center justify-center">
        <div>{{ maliciousAcitivity.id }}</div>
        <div>{{ maliciousAcitivity.user?.username }}</div>
        <div class="w-100">{{ maliciousAcitivity.activityType?.info }}</div>
        <div>{{ maliciousAcitivity.info }}</div>
        <div>{{ maliciousAcitivity.ipAddress }}</div>
      </div>
    </div>
  </div>
</template>
