export type Game = {
  id: number
  name: string
  description: string
  postedDate: Date
  imagePath: string
  rating: number
  tags: string[]
  developer: string
}

export function createGame(data: Partial<Game> = {}): Game {
  return {
    id: data.id ?? -1,
    name: data.name ?? 'NoName',
    description: data.description ?? 'NoDesc',
    postedDate: data.postedDate ?? new Date(),
    imagePath: data.imagePath ?? '../../assets/logo.png',
    rating: data.rating ?? 0.0,
    tags: data.tags ?? [],
    developer: data.developer ?? 'NoDev',
  }
}
