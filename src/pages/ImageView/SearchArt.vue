<script setup lang="ts">
import { useGameSearchSortingOptions } from '@/composables/useGameSearchSortingOptions'
import { computed, ref } from 'vue'

const {
  areSortingOptionsOpen,
  selectedSortingOption,
  revealSortingOptions,
  sortingOptionsClasses,
  getSelectedSortingOptionClasses,
  selectSorting,
  currentSorting,
} = useGameSearchSortingOptions()
</script>
<template>
  <!--Searching window, flex-col for sorting-->
  <div class="retro-window w-4/5 m-auto flex flex-col p-5 gap-4">
    <!-- Search bar-->
    <div class="flex flex-row comic-neue items-stretch">
      <input
        class="flex-1 border bg-[#bfedef] pt-0.5 pb-0.5 pr-2 pl-2"
        type="text"
        placeholder="search..."
      />
      <button
        class="bg-[#F4B5EA] text-[#98518E] border-l-0 border border-black hover:bg-[#e6a7dc] active:bg-[#d496ca] pt-0.5 pb-0.5 pr-5 pl-5 hover:cursor-pointer"
      >
        search
      </button>
    </div>

    <!--sort by [button]s that reveals the sort by buttons below it -->
    <div class="flex flex-row comic-neue">
      <button @click="revealSortingOptions()" v-bind:class="sortingOptionsClasses">
        sort by: {{ currentSorting }}
      </button>
    </div>

    <!--The different sort, filters, etc. But for now only sort by-->

    <!-- sort by: options -->
    <div v-show="areSortingOptionsOpen" class="flex flex-row gap-2">
      <span class="pr-2 self-center">sort by: </span>
      <button @click="selectSorting(1)" :class="getSelectedSortingOptionClasses(1)">
        relevant
      </button>
      <button @click="selectSorting(2)" :class="getSelectedSortingOptionClasses(2)">rating</button>
    </div>
  </div>
</template>
