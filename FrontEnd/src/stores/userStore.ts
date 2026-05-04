import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'
import { createUser, type User } from '@/data/User'
import { useShowProfileAndHideLogin } from '@/composables/useShowProfileAndHideLogin'

export const useUserStore = defineStore('userStore', () => {
  const user = ref<User>(createUser())
  const { logIn } = useShowProfileAndHideLogin()

  if (localStorage.getItem('activeUser')) {
    logIn()
    user.value = JSON.parse(localStorage.getItem('activeUser') ?? '')
  } else {
    user.value = createUser()
  }

  watch(
    user,
    (userVal) => {
      localStorage.setItem('activeUser', JSON.stringify(userVal))
    },
    { deep: true },
  )

  return {
    user,
  }
})
