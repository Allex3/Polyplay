<script setup lang="ts">
import { computed, ref } from 'vue'

console.log(window.innerWidth)

const isOnLargeMonitor = computed(() => (window.innerWidth > 880 ? true : false))
</script>

<template>
  <div class="retro-window flex flex-col items-center h-120 w-10/12 md:w-2/3 relative gap-5 m-auto">
    <img src="/assets//Homepage/Polyplay.png" alt="Polyplay" class="logoText" />
    <span
      >Discover fun indie games and build communities with similar players. Play while chatting,
      totally doable. You can ask players for tips on real time!!</span
    >
    <router-link to="/games" class="text-center pt-1 b b1 hover:cursor-pointer hover_scale"
      >Discover Games
    </router-link>
    <router-link to="/" class="text-center b b2 pt-1 hover:cursor-pointer hover_scale"
      >Discover People
    </router-link>

    <img
      id="bigStar"
      v-show="isOnLargeMonitor"
      class=""
      src="/assets//Homepage/StarBig.png"
      alt="BigStar"
    />
    <img id="star2" v-show="isOnLargeMonitor" src="/assets//Homepage/Star2.png" alt="Star 2" />
    <img id="star3" v-show="isOnLargeMonitor" src="/assets//Homepage/Star3.png" alt="Star 3" />
  </div>
</template>

<style scoped>
#bigStar {
  position: relative;
  scale: 0.5;
  margin-top: -15rem;
  align-self: baseline;
  left: -18rem;
  z-index: 100;
}

#star2 {
  position: relative;
  scale: 0.5;
  align-self: baseline;
  margin-top: -34rem;
  left: 38rem;
  z-index: 100;
}

#star3 {
  position: relative;
  align-self: baseline;
  scale: 0.5;
  margin-top: -58.67rem;
  left: 37rem;
  z-index: 1000;
}

span {
  font-size: 1.3rem;
  padding: 1em;
  text-align: center;
  text-wrap-mode: wrap;
}

.logoText {
  width: 12rem;
  margin-top: 4rem;
  margin-bottom: -1rem;
}

.b {
  width: 10em;
  height: 2em;
  font-size: 1.2rem;
  background: linear-gradient(90deg, #bfedef 0%, #e372d2 100%);
  border: 0.1rem solid black;
}

.b1 {
  background: linear-gradient(90deg, #bfedef 0%, #e372d2 100%);
}

.b2 {
  background: linear-gradient(90deg, #e372d2 0%, #bfedef 100%);
}
</style>
