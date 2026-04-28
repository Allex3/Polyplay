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

export type User = {
  username: string
  email: string
  password: string
  wantsNewsletter: boolean
  favoriteGames: number[]
  following: string[]
  followers: string[]
  pronouns: string
}

export function createUser(data: Partial<User> = {}): User {
  return {
    username: data.username ?? '0',
    email: data.email ?? '',
    password: data.password ?? '',
    wantsNewsletter: data.wantsNewsletter ?? false,
    favoriteGames: data.favoriteGames ?? [],
    following: data.following ?? [],
    followers: data.followers ?? [],
    pronouns: data.pronouns ?? 'they/them',
  }
}
