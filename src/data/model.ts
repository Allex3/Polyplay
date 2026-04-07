export type Game = {
  id: number
  name: string
  description: string
  postedDate: Date
  rating: number
  mainTag: string
  developer: string
}

export function createGame(data: Partial<Game> = {}): Game {
  return {
    id: data.id ?? -1,
    name: data.name ?? 'NoName',
    description: data.description ?? 'NoDesc',
    postedDate: data.postedDate ?? new Date(),
    rating: data.rating ?? 0.0,
    mainTag: data.mainTag ?? 'NoTags',
    developer: data.developer ?? 'NoDev',
  }
}
