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
import { computed, reactive, ref } from 'vue'
import { createGame, type Game } from '@/data/model'
import apiService from '@/api/apiService'

ChartJS.register(Title, Tooltip, Legend, BarElement, CategoryScale, LinearScale)

var chartOptions: any = { responsive: true }

var chartDataLabels: string[] = []
var chartDataDatasets: any[] = [{}]
chartDataDatasets[0].data = []

const chartData = ref({})
var games = (await apiService.games.getGames()).data

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

ChartJS.defaults.color = '#f4b5ea'
ChartJS.defaults.borderColor = '#000'

chartData.value = { labels: chartDataLabels, datasets: chartDataDatasets }

async function updateData(): Promise<void> {
  chartDataLabels = []
  chartDataDatasets = [{}]
  chartDataDatasets[0].data = []

  games = (await apiService.games.getGames()).data
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

  ChartJS.defaults.color = '#f4b5ea'
  ChartJS.defaults.borderColor = '#000'
  chartData.value = { labels: chartDataLabels, datasets: chartDataDatasets }
}

function addTestGames() {
  //TODO add test data in server with a test method in API service
}
</script>

<template>
  <Bar class="text-[#000000]" :options="chartOptions" :data="chartData" />
  <button
    @click="addTestGames"
    class="hover:cursor-pointer hover:scale-110 flex m-auto p-2 bg-[#bfedef] border-2 mb-3"
  >
    Add Test Games
  </button>
</template>
