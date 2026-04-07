<script setup lang="ts">
import { ref } from 'vue'
</script>

<template>
  <div class="retro-window flex flex-col items-center">
    <img src="/assets//Homepage/Polyplay.png" alt="Polyplay" class="logoText" />
    <span
      >Discover fun indie games and build communities with similar players. Play while chatting,
      totally doable. You can ask players for tips on real time!!</span
    >
    <router-link to="/"
      ><button class="b b1 hover:cursor-pointer hover_scale">Discover Games</button>
    </router-link>
    <router-link to="/"
      ><button class="b b2 hover:cursor-pointer hover_scale">Discover People</button>
    </router-link>

    <img id="bigStar" src="/assets//Homepage/StarBig.png" alt="BigStar" />
    <img id="star2" src="/assets//Homepage/Star2.png" alt="Star 2" />
    <img id="star3" src="/assets//Homepage/Star3.png" alt="Star 3" />
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
  z-index: 100;
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

.retro-window {
  gap: 5%;
  width: 50rem;
  height: 27rem;
  position: relative;
  margin: auto;
}
</style>
