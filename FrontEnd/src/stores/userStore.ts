import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'
import { createJwtToken, createUser, type JwtToken, type User } from '@/data/User'
import { useShowProfileAndHideLogin } from '@/composables/useShowProfileAndHideLogin'
import { USER_ROLE } from '@/data/Roles'
import { useUserRoles } from '@/composables/useUserRoles'

export const useUserStore = defineStore('userStore', () => {
  const user = ref<User>(createUser())
  const jwtToken = ref<JwtToken>(createJwtToken())
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

  if (localStorage.getItem('jwtToken')) {
    jwtToken.value = JSON.parse(localStorage.getItem('jwtToken') ?? '')
  } else {
    jwtToken.value = createJwtToken()
  }

  watch(
    jwtToken,
    (jwtTokenVal) => {
      localStorage.setItem('jwtToken', JSON.stringify(jwtTokenVal))
    },
    { deep: true },
  )

  return {
    user,
    jwtToken,
  }
})
