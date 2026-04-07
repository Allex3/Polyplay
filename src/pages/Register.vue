<template>
  <div class="retro-window flex flex-col gap-5 items-center">
    <span style="font-size: 2rem; margin-top: 4rem; margin-bottom: -1rem">Register</span>
    <form @submit.prevent="login" class="flex flex-col items-center gap-5 p-5">
      <div class="flex flex-col gap-1.5">
        <label for="username">Username</label>
        <input class="login_input" name="username" v-model="username" placeholder="username" />
      </div>
      <div class="flex flex-col gap-1.5">
        <label for="email">E-mail</label>
        <input
          class="login_input"
          name="email"
          v-model="email"
          placeholder="e-mail@something.me"
          type="e-mail"
        />
      </div>
      <div class="flex flex-col gap-1.5">
        <label for="password">Password</label>
        <input
          class="login_input"
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
      <input class="register hover_scale hover:cursor-pointer" type="submit" value="Register" />
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

.retro-window {
  width: 30rem;
  height: 35rem;
  position: relative;
  margin: auto;
}
</style>

<script setup lang="ts">
import { ref } from 'vue'

const username = ref('')
const email = ref('')
const password = ref('')
const newsletter = ref(false)

async function login() {
  // no clue, no backend yet :(
}
</script>
