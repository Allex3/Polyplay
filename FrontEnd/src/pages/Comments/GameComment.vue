<script setup lang="ts">
import type { GameComment } from '@/data/GameComment'

const props = defineProps<{
  gameComment: GameComment
}>()
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
    <!-- maybe it's made by Vue, with like a button that changes classes...-->
  </div>
</template>
