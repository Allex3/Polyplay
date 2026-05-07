import { USER_ROLE } from '@/data/Roles'
import { useUserStore } from '@/stores/userStore'
import { ref } from 'vue'

const isUserAdmin = ref(false)

export function useUserRoles() {
  const makeAdmin = () => (isUserAdmin.value = true)
  const removeAdmin = () => (isUserAdmin.value = false)

  return { isUserAdmin, makeAdmin, removeAdmin }
}
