<script setup lang="ts">
import { createGameComment } from '@/data/GameComment'
import { ref } from 'vue'
import { useUserStore } from '@/stores/userStore'
import apiService from '@/api/apiService'
import { usePostPutApiCallWithErrors } from '@/composables/usePostPutApiCallWithErrors'

const userStore = useUserStore()

const { validateInput } = usePostPutApiCallWithErrors()

const props = defineProps<{
  gameId: number
}>()

const emit = defineEmits(['postedComment'])

const currentlyPosting = ref(false)

const currentComment = ref(createGameComment({ userId: userStore.user.id, gameId: props.gameId }))

const isInvalidFormat = ref(false) // the comment is >1000 chars or empty
const errorText = ref('')
const isPostedSuccessfully = ref(false)
const successText = ref('')

async function postComment(): Promise<void> {
  currentlyPosting.value = true
  console.log('aaa')
  let apiResponse = await apiService.gamesComments.postGameComment(currentComment.value)
  if (
    validateInput(
      apiResponse,
      isInvalidFormat,
      errorText,
      isPostedSuccessfully,
      successText,
      'Posted Successfully!',
    )
  ) {
    currentComment.value.body = '' // empty it if it posted
    emit('postedComment')
  }

  currentlyPosting.value = false
}
</script>
<template>
  <div class="flex flex-col items-center">
    <form @submit.prevent="postComment" class="flex flex-row w-full">
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
        value="Post"
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
