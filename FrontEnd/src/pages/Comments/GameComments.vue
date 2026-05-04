<script setup lang="ts">
import { ref } from 'vue'
import apiService from '@/api/apiService'
import { createGameComment, type GameComment as GameCom } from '@/data/GameComment'
import GameComment from './GameComment.vue'
import GameCommentPost from './GameCommentPost.vue'

const props = defineProps<{
  gameId: number
}>()
const gameCommentsApiResponse = await apiService.gamesComments.getGameComments(props.gameId)

const gameComments = ref<GameCom[]>([])

gameComments.value = gameCommentsApiResponse // comments in an
</script>
<template>
  <div>
    <div class="-mb-17 text-lg p-4 ml-10 md:ml-0">Comments</div>

    <div class="w-5/6 retro-window md:w-220 gap-5 m-auto flex flex-col mt-12 pl-4 pr-4 pt-5 pb-5">
      <!-- comment input (post)-->
      <GameCommentPost :game-id="gameId" />
      <div v-for="gameComment in gameComments" :key="gameComment.id">
        <GameComment :game-comment="gameComment" />
      </div>
    </div>
  </div>
</template>
