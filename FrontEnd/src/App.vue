<script setup lang="ts">
import { useShowProfileAndHideLogin } from './composables/useShowProfileAndHideLogin'

const { isUserLoggedIn } = useShowProfileAndHideLogin()
</script>
<template>
  <div
    class="sticky top-0 flex-col gap-2 flex md:flex-row md:gap-10 items-center z-100 bg-[#6c4366] mb-10 border-b-2"
  >
    <router-link to="/" class="hover_scale single_link w-30 md:w-40"
      ><img src="../assets/logo.png" alt="Logo" class="w-32"
    /></router-link>
    <div
      class="sticky top-0 grid grid-cols-3 gap-4 text-center md:flex md:flex-row md:gap-10 items-center"
    >
      <router-link to="/" class="hover_scale single_link">Home</router-link>
      <router-link to="/games" class="hover_scale single_link">Games</router-link>
      <router-link v-show="!isUserLoggedIn" to="/Register" class="hover_scale single_link"
        >Register</router-link
      >
      <router-link v-show="!isUserLoggedIn" to="/Login" class="hover_scale single_link"
        >Login</router-link
      >
      <router-link to="/user/table" class="hover_scale single_link">Dashboard</router-link>
      <router-link to="/stats" class="hover_scale single_link">Statistics</router-link>
    </div>
  </div>
  <router-view />
</template>

<style scoped>
.single_link {
  transition: -webkit-text-stroke 0.5s;
  font-size: 1.5rem;
}
.single_link:hover {
  -webkit-text-stroke: 0.05rem #f4b5ea;
}

.slide-fade-enter-active {
  transition: all 0.3s ease-out;
}

.slide-fade-leave-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}

.slide-fade-enter-from,
.slide-fade-leave-to {
  transform: translateX(50rem);
  opacity: 0;
}
</style>
