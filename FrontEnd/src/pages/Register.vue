<script setup lang="ts">
import { ref } from 'vue'
import { useUserStore } from '@/stores/userStore'
import { createUser } from '@/data/User'
import { useRouter } from 'vue-router'

const userStore = useUserStore()

const registerFailed = ref(false)
const registerFailedErrorText = ref('')

const router = useRouter()

const username = ref('')
const email = ref('')
const password = ref('')
const newsletter = ref(false)

function registerInputsAreValid(): boolean {
  registerFailed.value = false

  if (registerFailed.value) return false //it fails

  return true
}

async function register() {
  if (!registerInputsAreValid()) return
  /* TODO REFACTOR IN API
  userStore.addUser(
    createUser({
      username: username.value,
      password: password.value,
      email: email.value,
      wantsNewsletter: newsletter.value,
    }),
  )*/

  router.push('/login')
}
</script>

<template>
  <div class="retro-window flex flex-col gap-5 w-5/6 md:2/5 items-center m-auto relative">
    <span style="font-size: 2rem; margin-top: 4rem; margin-bottom: -1rem">Register</span>
    <form @submit.prevent="register" class="flex flex-col items-center gap-5 p-5">
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
        <label for="email">E-mail</label>
        <input
          id="inputEmail"
          class="retro-input"
          name="email"
          v-model="email"
          placeholder="e-mail@something.me"
          type="e-mail"
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
      <div class="flex flex-row gap-3 items-center">
        <input type="checkbox" id="newsletter" name="newsletter" v-model="newsletter" />
        <label for="newsletter">Subscribe to the new creations newsletter</label>
      </div>
      <span class="text-[#ee0a0a] text-center" id="errorText" v-show="registerFailed">{{
        registerFailedErrorText
      }}</span>
      <input
        id="register"
        @submit="register()"
        class="register hover_scale hover:cursor-pointer"
        type="submit"
        value="Register"
      />
    </form>
  </div>
</template>

<style scoped>
label {
  font-size: 1.2rem;
}

.register {
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
