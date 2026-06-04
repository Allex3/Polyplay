<script setup lang="ts">
var jeff: any = document.getElementById('jeff')

function hmhm() {
  jeff = document.getElementById('jeff')

  jeff.requestFullscreen()
}
</script>
<template>
  <div class="w-full h-full absolute ml-0">
    <video @click="hmhm()" loop autoplay id="jeff" class="w-full h-full absolute" alt="hmmhmhmhmh">
      <source src="../../../assets/jeff.mp4" type="video/mp4" />
    </video>
    <audio autoplay loop>
      <source src="../../../assets/jeff.mp3" type="audio/mpeg" />
    </audio>
    <div class="absolute flex justify-center items-center w-full h-full">
      <span class="text-5xl text-red-700">STOP SPAMMING THE API</span>
    </div>
  </div>
</template>
