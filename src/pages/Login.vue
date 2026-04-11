<template>
  <div class="retro-window flex flex-col gap-5 items-center">
    <span style="font-size: 2rem; margin-top: 4rem; margin-bottom: -1rem">Login</span>
    <form @submit.prevent="login" class="flex flex-col items-center gap-5 p-5">
      <div class="flex flex-col gap-1.5">
        <label for="username">Username</label>
        <input class="retro-input" name="username" v-model="username" placeholder="username" />
      </div>
      <div class="flex flex-col gap-1.5">
        <label for="password">Password</label>
        <input
          class="retro-input"
          name="password"
          v-model="password"
          placeholder="password"
          type="password"
        />
      </div>
      <span
        >Don't have an account?
        <router-link to="/Register" class="register_link hover_scale"
          >Register here</router-link
        ></span
      >
      <input class="login hover_scale hover:cursor-pointer" type="submit" value="Login" />
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
