<script setup lang="ts"></script>

<template>
  <div id="nav" class="flex flex-row gap-10 p-2 items-center">
    <router-link to="/" class="hover_scale single_link"><img src="/assets/logo.png" alt="Logo" class="logo" /></router-link>
    <router-link to="/" class="hover_scale single_link">Home</router-link>
    <router-link to="/Register" class="hover_scale single_link">Register</router-link>
    <router-link to="/Login" class="hover_scale single_link">Login</router-link>
    <router-link to="/admin/table" class="hover_scale single_link">Admin Dashboard</router-link>
  </div>
  <router-view />
</template>

<style scoped>
.logo {
  width: 10rem;
}

.single_link {
  transition: -webkit-text-stroke 0.5s;
  font-size: 1.5rem;
}
.single_link:hover {
  -webkit-text-stroke: 0.05rem #f4b5ea;
}
</style>
