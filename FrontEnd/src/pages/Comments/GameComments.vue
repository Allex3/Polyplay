<script setup lang="ts">
import { reactive, ref } from 'vue'
import apiService from '@/api/apiService'
import { createGameComment, type GameComment as GameCom } from '@/data/GameComment'
import GameComment from './GameComment.vue'
import GameCommentPost from './GameCommentPost.vue'
import { useUserStore } from '@/stores/userStore'
import { createUserActivity, USER_ACTIVITIES } from '@/data/UserActivity'

const userStore = useUserStore()

const props = defineProps<{
  gameId: number
}>()
const gameComments = ref<GameCom[]>([])

const showEdits = reactive<any>({})

updateComments()

async function updateComments() {
  gameComments.value = await apiService.gamesComments.getGameComments(props.gameId) // comments in a list
  if (Object.keys(showEdits.value).length == 0) {
    //if its empty, otherwise don't modify it
    gameComments.value.forEach((game) => {
      showEdits[game.id] = false // don't show its edit at first
    })
  }
}

async function deleteGame(gameCommentId: number) {
  apiService.gamesComments.deleteGameComment(gameCommentId)
  apiService.userActivity.postUserActivity(
    createUserActivity({
      userId: userStore.user.id,
      activityTypeId: USER_ACTIVITIES.DELETE_GAME_COMMENT,
    }),
  )
  setTimeout(updateComments, 200) // wait 200ms so that the list updates
}
</script>
<template>
  <div>
    <div class="-mb-17 text-lg p-4 ml-10 md:ml-0">Comments</div>

    <div class="w-5/6 retro-window md:w-220 gap-5 m-auto flex flex-col mt-12 pl-4 pr-4 pt-5 pb-5">
      <!-- comment input (post), tied to this game and current active user-->
      <!-- will (probably) get an error if no logged in user -->
      <GameCommentPost
        :edit-mode="false"
        :comment="createGameComment({ gameId: props.gameId, userId: userStore.user.id })"
        @posted-comment="updateComments"
      />
      <div v-for="gameComment in gameComments" :key="gameComment.id">
        <!-- reactive show or hide the edit box based on if it's editing now or not-->
        <GameComment
          :game-comment="gameComment"
          @started-edit="showEdits[gameComment.id] = true"
          @canceled-edit="showEdits[gameComment.id] = false"
          @deleted-game="deleteGame(gameComment.id)"
        />
        <!-- below it, a box to edit the game... that is shown only when we press the edit button -->
        <GameCommentPost
          v-show="
            showEdits[gameComment.id] &&
            userStore.user.id ==
              gameComment.userId /* even if forced, don't show if it's not same user TODO look later how to make it safe login auth idk*/
          "
          :edit-mode="true"
          :comment="createGameComment(gameComment) /*copy*/"
          @posted-comment="updateComments"
        />
      </div>
    </div>
  </div>
</template>
