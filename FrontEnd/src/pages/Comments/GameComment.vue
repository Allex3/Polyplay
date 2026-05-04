<script setup lang="ts">
import type { GameComment } from '@/data/GameComment'
import { useUserStore } from '@/stores/userStore'
import { ref } from 'vue'

const props = defineProps<{
  gameComment: GameComment
}>()

const userStore = useUserStore()

const emit = defineEmits(['startedEdit', 'canceledEdit'])
const isEditing = ref(false)
const editButtonText = ref('Edit')

function editComment(): void {
  if (isEditing.value) {
    emit('canceledEdit')
    isEditing.value = false
    editButtonText.value = 'Edit'
    return
  }
  emit('startedEdit')
  isEditing.value = true
  editButtonText.value = 'Cancel Edit'
}
</script>

<template>
  <div class="border-2 p-1.5 flex flex-col">
    <!--(photo +) username (photo not yet) -->
    <div class="text-xl comic-neue-bold p-0.5">{{ gameComment.userId }}</div>
    <!-- Comment body-->
    <div class="text-sm comic-neue p-0.5 wrap-break-word">
      {{ gameComment.body }}
    </div>
    <!-- overflow-hidden text-ellipsis whitespace-nowrap -->
    <!-- if I add these classes, how do i make "show more?"-->
    <!-- maybe it's made by Vue code, with like a button that changes classes...-->
    <button
      v-show="gameComment.userId == userStore.user.id"
      @click="editComment"
      class="text-left w-fit comic-neue-bold p-0.5 hover:cursor-pointer text-[#905a88] hover:text-[#6e4467] active:text-[#563550]"
    >
      {{ editButtonText }}
    </button>
  </div>
</template>
