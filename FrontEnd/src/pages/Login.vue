<template>
  <div class="retro-window flex flex-col gap-5 w-5/6 md:2/5 items-center m-auto relative">
    <span style="font-size: 2rem; margin-top: 4rem; margin-bottom: -1rem">Login</span>
    <form @submit.prevent="login" class="flex flex-col items-center gap-5 p-5">
      <div class="flex flex-col gap-1.5">
        <label for="username">Username</label>
        <input
          id="inputUsername"
          class="retro-input"
          name="username"
          v-model="username"
          placeholder="username"
        />
      </div>
      <div class="flex flex-col gap-1.5">
        <label for="password">Password</label>
        <input
          id="inputPassword"
          class="retro-input"
          name="password"
          v-model="password"
          placeholder="password"
          type="password"
        />
      </div>
      <span class="text-[#ee0a0a] text-center" v-show="loginFailed">{{
        loginFailedErrorText
      }}</span>
      <span
        >Don't have an account?
        <router-link to="/Register" class="register_link hover_scale"
          >Register here</router-link
        ></span
      >
      <input
        @submit="login"
        id="login"
        class="login hover_scale hover:cursor-pointer"
        type="submit"
        value="Login"
      />
    </form>
  </div>
</template>

<style scoped>
label {
  font-size: 1.2rem;
}

.register_link {
  color: #a8368d;
  transition: -webkit-text-stroke 0.5s;
}
.register_link:hover {
  -webkit-text-stroke: 0.01rem black;
}

.login {
  width: 6rem;
  background-color: #bfedef;
  padding: 0.1rem 0.3rem;
  border: 0.1rem solid black;
  margin-top: 0.5rem;
}

.login_input {
  width: 15rem;
}
</style>

<script setup lang="ts">
import { useUserStore } from '@/stores/userStore'
import { ref } from 'vue'
import { useShowProfileAndHideLogin } from '@/composables/useShowProfileAndHideLogin'
import { useRouter } from 'vue-router'
import { createUser } from '@/data/User'

const { logIn } = useShowProfileAndHideLogin()

const userStore = useUserStore()

const loginFailed = ref(false)
const loginFailedErrorText = ref('')

const router = useRouter()

const username = ref('')
const password = ref('')

function loginInputsAreValid(): boolean {
  /*loginFailed.value = false

  if (!userStore.userExists(username.value)) {
    loginFailedErrorText.value = 'User with that username does not exist'
    loginFailed.value = true
  } else if (!(userStore.getUser(username.value).password == password.value)) {
    loginFailedErrorText.value = 'Password is incorrect.'
    loginFailed.value = true
  }

  if (loginFailed.value) return false
*/
  return true
}

async function login() {
  if (!loginInputsAreValid()) return

  userStore.user = createUser({ username: 'alex', password: 'alex.2005' })

  logIn()
  router.push('/user/table')
}
</script>
