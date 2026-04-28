import apiService from '@/api/apiService'
import { ref } from 'vue'

var visibleGamesOnPage: number = 12
const page = ref(1)

var gamesData = await apiService.games.getGames(page.value, visibleGamesOnPage)
const visibleGames = ref(gamesData.games) // games on this page

const totalPages = ref(Math.ceil(gamesData.totalGames / visibleGamesOnPage))

async function goToPage(pageNumber: number) {
  page.value = pageNumber
  if (pageNumber < 1) page.value = 1
  if (pageNumber > totalPages.value) page.value = totalPages.value

  gamesData = await apiService.games.getGames(page.value, visibleGamesOnPage)
  visibleGames.value = gamesData.games
}

//TODO use websocket to update the games automatically when the backend inserts more games..
// what will happen: update total pages, update the page we're currently on
// (since the newly inserted entities may be on this page)

function resetPagination(visibleGamesOnPageCurrently: number) {
  page.value = 1
  visibleGamesOnPage = visibleGamesOnPageCurrently
  totalPages.value = Math.ceil(gamesData.totalGames / visibleGamesOnPage)
  goToPage(page.value) // go to page 1
}

export function usePagination(visibleGamesOnPageCurrently: number) {
  // first reset the pagination, as when we use it we use it on another view..
  resetPagination(visibleGamesOnPageCurrently)

  return { visibleGames, visibleGamesOnPage, page, goToPage, totalPages }
}
