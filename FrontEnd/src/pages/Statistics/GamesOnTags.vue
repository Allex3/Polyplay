<script setup lang="ts">
import { Bar } from 'vue-chartjs'
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale,
} from 'chart.js'
import { computed, getCurrentInstance, reactive, ref, useTemplateRef } from 'vue'
import { createGame, type Game } from '@/data/model'
import apiService from '@/api/apiService'

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

var chartOptions: any = { responsive: true }

var chartDataLabels: string[] = []
var chartDataDatasets: any[] = [{}]
chartDataDatasets[0].data = []

const barKey = ref(1) // t oforce stats to re-render

const chartData = ref<any>({})

ChartJS.defaults.color = '#f4b5ea'
ChartJS.defaults.borderColor = '#000'

const MAX_GAMES_ON_PAGE = 20

await updateData()

async function updateData(): Promise<any> {
  chartDataLabels = []
  chartDataDatasets = [{}]
  chartDataDatasets[0].data = []

  let games: [] = [] // empty array now
  let pageNumber: number = 1
  let currentPageGames = (await apiService.games.getGames(1, MAX_GAMES_ON_PAGE)).games
  while (currentPageGames.length > 0) {
    // run this while there are games in the API, to get them all
    games.push.apply(games, currentPageGames)
    pageNumber++
    currentPageGames = (await apiService.games.getGames(pageNumber, MAX_GAMES_ON_PAGE)).games
  }

  games.forEach((game: Game) => {
    let currentTag = game.mainTag ?? 'No Tag'
    if (!chartDataLabels.includes(currentTag)) // new tag
    {
      chartDataLabels.push(currentTag) // push the new tag
      chartDataDatasets[0].data.push(1) // push the new tag at the end
      return
    }

    // for each game also see what tag it is to add to that specific dataset position
    let currentTagIndex = chartDataLabels.findIndex((tag) => tag === currentTag)
    chartDataDatasets[0].data[currentTagIndex] += 1 // one more game
  })
  chartDataDatasets[0].label = 'Game Tags'
  chartDataDatasets[0].backgroundColor = '#f4b5ea'

  chartData.value.labels = chartDataLabels
  chartData.value.datasets = chartDataDatasets

  barKey.value++ // force stats to re-render
}

const isGeneratingGames = ref(false)

function generateTestGames() {
  apiService.games.generateTestGames(updateData) // update data on each generated request
  isGeneratingGames.value = true
}
function stopGeneratingTestGames() {
  apiService.games.stopGeneratingTestGames()
  isGeneratingGames.value = false
}
</script>

<template>
  <Bar :key="barKey" :options="chartOptions" :data="chartData" />
  <button
    @click="generateTestGames"
    class="hover:cursor-pointer hover:scale-110 flex m-auto p-2 bg-[#bfedef] border-2 mb-3"
  >
    Generate Test Games
  </button>
  <button
    @click="stopGeneratingTestGames"
    v-show="isGeneratingGames"
    class="hover:cursor-pointer hover:scale-110 flex m-auto p-2 bg-[#bfedef] border-2 mb-3"
  >
    Stop Generating
  </button>
</template>
