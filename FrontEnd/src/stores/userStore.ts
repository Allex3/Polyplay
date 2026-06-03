import { ref, computed, watch } from 'vue'
import { defineStore } from 'pinia'
import {
  createJwtToken,
  createTokenRequest,
  createUser,
  type JwtToken,
  type User,
} from '@/data/User'
import { useShowProfileAndHideLogin } from '@/composables/useShowProfileAndHideLogin'
import { USER_ROLE } from '@/data/Roles'
import { useUserRoles } from '@/composables/useUserRoles'
import apiService from '@/api/apiService'

export const useUserStore = defineStore('userStore', () => {
  const user = ref<User>(createUser())
  const jwtToken = ref<JwtToken>(createJwtToken())
  const { logIn, logOut } = useShowProfileAndHideLogin()

  if (localStorage.getItem('activeUser')) {
    logIn()
    user.value = JSON.parse(localStorage.getItem('activeUser') ?? '')
    if (user.value.roles.includes(USER_ROLE.ADMIN)) useUserRoles().makeAdmin()
  }

  if (localStorage.getItem('jwtToken')) {
    jwtToken.value = JSON.parse(localStorage.getItem('jwtToken') ?? '')
  }

  refreshJwtToken() // try to refresh it, will fail if it's invalid or didn't expire yet

  // set them to local storage when they change
  watch(
    jwtToken,
    (jwtTokenVal) => {
      localStorage.setItem('jwtToken', JSON.stringify(jwtTokenVal))
    },
    { deep: true },
  )

  watch(
    user,
    (userVal) => {
      localStorage.setItem('activeUser', JSON.stringify(userVal))
    },
    { deep: true },
  )

  async function refreshJwtToken(): Promise<void> {
    if (!isJwtTokenValid()) {
      jwtToken.value = createJwtToken()
      user.value = createUser()
      return
    }

    // expired
    if (hasTokenExpired()) {
      let refreshResponse: any = await apiService.users.refreshJwtToken(
        createTokenRequest({
          token: jwtToken.value.token,
          refreshToken: jwtToken.value.refreshToken,
        }),
      )
      if (refreshResponse.success) jwtToken.value = refreshResponse.usersData
    }
  }

  function isJwtTokenValid(): boolean {
    return !(
      jwtToken.value === undefined ||
      jwtToken.value.token == '' ||
      jwtToken.value.refreshToken == ''
    )
  }

  function hasTokenExpired(): boolean {
    if (!isJwtTokenValid) return false // didn't expire, it's just not valid

    let currentDate = new Date()
    let currentDateUtcNumber = Date.UTC(
      currentDate.getUTCFullYear(),
      currentDate.getUTCMonth(),
      currentDate.getUTCDate(),
      currentDate.getUTCHours(),
      currentDate.getUTCMinutes(),
      currentDate.getUTCSeconds(),
    )
    let currentDateUtc = new Date(currentDateUtcNumber)
    let tokenExpiryDate = new Date(jwtToken.value.expiresAt)

    return tokenExpiryDate < currentDateUtc
  }

  return {
    user,
    jwtToken,
    refreshJwtToken,
    isJwtTokenValid,
  }
})
