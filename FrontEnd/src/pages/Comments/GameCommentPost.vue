<script setup lang="ts">
import { createGameComment, type GameComment } from '@/data/GameComment'
import { computed, ref } from 'vue'
import { useUserStore } from '@/stores/userStore'
import apiService from '@/api/apiService'
import { usePostPutApiCallWithErrors } from '@/composables/usePostPutApiCallWithErrors'
import { createUserActivity, USER_ACTIVITIES } from '@/data/UserActivity'

const { validateInput, errorText, isPostedSuccessfully, isInvalidFormat, successText } =
  usePostPutApiCallWithErrors()

const props = defineProps<{
  editMode: boolean
  comment: GameComment
}>()

const emit = defineEmits(['postedComment'])

const currentlyPosting = ref(false)

const submitButtonText = computed(() => (props.editMode ? 'Edit' : 'Post'))

const currentComment = ref(props.comment) // clone

async function postComment() {
  let apiResponse = await apiService.gamesComments.postGameComment(currentComment.value)
  if (validateInput(apiResponse, 'Posted successfully :3')) {
    currentComment.value.body = '' // empty it if it posted
    apiService.userActivity.postUserActivity(
      createUserActivity({
        userId: useUserStore().user.id,
        activityTypeId: USER_ACTIVITIES.POST_GAME_COMMENT,
      }),
    )
    emit('postedComment')
  }
}
async function editComment() {
  let apiResponse = await apiService.gamesComments.putGameComment(currentComment.value)
  if (validateInput(apiResponse, 'Edited successfully :3')) {
    apiService.userActivity.postUserActivity(
      createUserActivity({
        userId: useUserStore().user.id,
        activityTypeId: USER_ACTIVITIES.PUT_GAME_COMMENT,
      }),
    )
    emit('postedComment')
  }
}

async function saveComment(): Promise<void> {
  currentlyPosting.value = true

  if (!props.editMode) {
    postComment()
  } else editComment()

  currentlyPosting.value = false
}
</script>
<template>
  <div class="flex flex-col items-center">
    <form @submit.prevent="saveComment" class="flex flex-row w-full">
      <input
        type="textarea"
        v-model="currentComment.body"
        name="commentBodyInput"
        placeholder="Enter Comment..."
        class="comic-neue flex-1 resize-none overflow-hidden wrap-break-word border-2 p-2 bg-[#bfedef]"
      />
      <input
        :disabled="currentlyPosting"
        type="submit"
        class="p-2 comic-neue-bold border-2 border-l-0 bg-[#f4b5ea] hover:cursor-pointer hover:bg-[#e4a6d9]"
        :value="submitButtonText"
      />
    </form>
    <span class="wrap-break-word whitespace-pre-line text-[#d01010]" v-show="isInvalidFormat">{{
      errorText
    }}</span>
    <span class="wrap-break-word text-[green]" v-show="isPostedSuccessfully">{{
      successText
    }}</span>
  </div>
</template>
