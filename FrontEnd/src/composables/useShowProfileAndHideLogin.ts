import { ref } from 'vue'

const isUserLoggedIn = ref(false)

export function useShowProfileAndHideLogin() {
  const logIn = () => (isUserLoggedIn.value = true)
  const logOut = () => (isUserLoggedIn.value = false)
  return { isUserLoggedIn, logIn, logOut }
}
