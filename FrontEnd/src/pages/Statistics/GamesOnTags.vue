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
import { createGame, type Game } from '@/data/Game'
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
  let gamesMainTagStats = (await apiService.games.getTagStats()).gamesData

  chartDataLabels = gamesMainTagStats.labels
  chartDataDatasets = [{}]
  chartDataDatasets[0].data = gamesMainTagStats.mainTagCount

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
