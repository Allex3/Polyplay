import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'
import { createUser, type User } from '@/data/User'

export const useUserStore = defineStore('userStore', () => {
  const user = ref<User>(createUser())

  if (localStorage.getItem('activeUser')) {
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
