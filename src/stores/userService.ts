import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'
import { createGame, type Game } from '@/data/model.ts'
import { createUser, type User } from '@/data/model.ts'

export const useUserService = defineStore('userService', () => {
  const user = ref<User>(createUser())
  const users = ref<User[]>([])

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
