import { computed, ref } from 'vue'

const areSortingOptionsOpen = ref(false)
const selectedSortingOption = ref(1)

function revealSortingOptions() {
  areSortingOptionsOpen.value = !areSortingOptionsOpen.value
  // at first don't show them, then when clicking the button we show them
  // also change how the sorting reveal button looks
}

const sortingOptionsClasses = computed(() => {
  if (areSortingOptionsOpen.value)
    // should change when the value it depends on (this) changes
    // open
    return 'bg-[#98518E] text-[#F4B5EA] pt-1 pb-1 pl-3 pr-3 hover:cursor-pointer border border-black'
  else
    return 'bg-[#F4B5EA] text-[#98518E] pt-1 pb-1 pl-3 pr-3 hover:cursor-pointer border border-black'
})

function getSelectedSortingOptionClasses(optionNumber: number): string {
  if (optionNumber == selectedSortingOption.value)
    // clicked option classes
    return 'bg-[#98518E] text-[#F4B5EA] pt-0.5 pb-0.5 pl-4 pr-4 comic-neue hover:cursor-pointer border border-black'
  return 'bg-[#F4B5EA] text-[#98518E] pt-0.5 pb-0.5 pl-4 pr-4 comic-neue hover:cursor-pointer border border-black'
}

function selectSorting(optionNumber: number): void {
  selectedSortingOption.value = optionNumber

  switch (optionNumber) {
    case 1:
      currentSorting.value = 'relevant'
      break
    case 2:
      currentSorting.value = 'rating'
      break
    default:
      currentSorting.value = 'relevant'
      break
  }
}

const currentSorting = ref('relevant')

export function useGameSearchSortingOptions() {
  return {
    areSortingOptionsOpen,
    selectedSortingOption,
    revealSortingOptions,
    sortingOptionsClasses,
    getSelectedSortingOptionClasses,
    selectSorting,
    currentSorting,
  }
}
