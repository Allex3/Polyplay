import { ref } from 'vue'
import { useUserRoles } from './useUserRoles'
import apiService from '@/api/apiService'
import { useUserStore } from '@/stores/userStore'
import { USER_ROLE } from '@/data/Roles'
import { createUser, type User } from '@/data/User'

const isUserLoggedIn = ref(false)

export function useShowProfileAndHideLogin() {
  const { isUserAdmin } = useUserRoles()
  let userStore = useUserStore()

  const logIn = async () => {
    if (userStore.user === undefined) return

    isUserLoggedIn.value = true
    const { makeAdmin } = useUserRoles()

    const user: User = userStore.user ?? createUser()

    let userRoles: string[] = (await apiService.users.getUserRoles(user.userName)).usersData // array of roles
    if (userRoles.includes('Admin')) {
      userStore.user.roles = [USER_ROLE.ADMIN, USER_ROLE.USER]
      makeAdmin()
    }
  }
  const logOut = () => {
    isUserLoggedIn.value = false
    if (userStore.user === undefined) return
  }
  return { isUserLoggedIn, logIn, logOut }
}
