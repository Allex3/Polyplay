<script setup lang="ts">
import apiService from '@/api/apiService'
import { useShowProfileAndHideLogin } from '@/composables/useShowProfileAndHideLogin'
import { type UserActivity } from '@/data/UserActivity'
import { useUserStore } from '@/stores/userStore'
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const { isUserLoggedIn } = useShowProfileAndHideLogin()

const userActivitiesLog = ref<UserActivity[]>(await apiService.userActivity.getUserActivities())

console.log(userActivitiesLog)

onMounted(() => {
  if (!isUserLoggedIn.value) {
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
      <div class="ml-10">Timestamp</div>
      <div>Extra Info</div>
      <div>IP</div>
    </div>
    <div v-for="userAcitivity in userActivitiesLog">
      <div class="flex flex-row gap-22 comic-neue items-center w-full text-center justify-center">
        <div>{{ userAcitivity.id }}</div>
        <div>{{ userAcitivity.user.username }}</div>
        <div class="w-100">{{ userAcitivity.activityType.info }}</div>
        <div class="w-100">
          {{ new Date(userAcitivity.activityTimestamp).toLocaleString() }}
        </div>
        <div>{{ userAcitivity.additionalInfo }}</div>
        <div>{{ userAcitivity.ipAddress }}</div>
      </div>
    </div>
  </div>
</template>
