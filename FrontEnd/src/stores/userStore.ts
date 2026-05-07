import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'
import { createUser, type User } from '@/data/User'
import { useShowProfileAndHideLogin } from '@/composables/useShowProfileAndHideLogin'
import { USER_ROLE } from '@/data/Roles'
import { useUserRoles } from '@/composables/useUserRoles'

export const useUserStore = defineStore('userStore', () => {
  const user = ref<User>(createUser())
  const { logIn } = useShowProfileAndHideLogin()

  if (localStorage.getItem('activeUser')) {
    logIn()
    user.value = JSON.parse(localStorage.getItem('activeUser') ?? '')
    if (user.value.roles.includes(USER_ROLE.ADMIN)) useUserRoles().makeAdmin()
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
