export type Game = {
  id: number
  name: string
  description: string
  postedDate: Date
  thumbnailPath: string
  rating: number
  mainTag: string
  developer: string
  isPublished: boolean
}

export function createGame(data: Partial<Game> = {}): Game {
  return {
    id: data.id ?? -1,
    name: data.name ?? '',
    description: data.description ?? '',
    postedDate: new Date(data.postedDate ?? new Date()) ?? new Date(),
    thumbnailPath: data.thumbnailPath ?? '../../assets/logo.png',
    rating: data.rating ?? 0.0,
    mainTag: data.mainTag ?? '',
    developer: data.developer ?? 'NoDev',
    isPublished: data.isPublished ?? false,
  }
}
