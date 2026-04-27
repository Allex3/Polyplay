import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'
import { createGame, type Game } from '@/data/model.ts'
import { createUser, type User } from '@/data/model.ts'

export const useUserRepo = defineStore('userService', () => {
  const user = ref<User>(createUser())
  const users = ref<User[]>([])

  if (localStorage.getItem('activeUser')) {
    user.value = JSON.parse(localStorage.getItem('activeUser') ?? '')
  } else {
    user.value = createUser()
  }

  if (localStorage.getItem('users')) {
    users.value = JSON.parse(localStorage.getItem('users') ?? '')
  } else {
    users.value = []
  }

  watch(
    user,
    (userVal) => {
      localStorage.setItem('activeUser', JSON.stringify(userVal))
    },
    { deep: true },
  )
  watch(
    users,
    (usersVal) => {
      // password of all users is visible in local storage, yes
      localStorage.setItem('users', JSON.stringify(usersVal))
    },
    { deep: true },
  )

  function resetUsers() {
    users.value = []
  }

  function userExists(username: string): boolean {
    const userIndex = users.value.findIndex((user) => user.username === username)
    if (userIndex == -1) return false
    return true
  }

  function getUser(username: string): User {
    const user = users.value.find((user) => user.username === username)
    if (user === undefined)
      throw new Error(`Get User: User with username ${username} does NOT exist`)
    return user
  }

  function addUser(user: User): void {
    users.value.push(user)
  }

  function updateUser(updatedUser: User): void {
    const userIndex = users.value.findIndex((user) => user.username === updatedUser.username)
    if (userIndex == -1)
      throw new Error(`Update User: User with username ${updatedUser.username} does NOT exist`)
    users.value[userIndex] = updatedUser
  }

  return {
    user,
    users,
    addUser,
    updateUser,
    getUser,
    resetUsers,
    userExists,
  }
})
