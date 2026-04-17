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
    postedDate: new Date(data.postedDate ?? new Date()) ?? new Date(),
    imagePath: data.imagePath ?? '../../assets/logo.png',
    rating: data.rating ?? 0.0,
    tags: data.tags ?? [],
    developer: data.developer ?? 'NoDev',
  }
}

export type User = {
  username: string
  email: string
  password: string
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
    favoriteGames: data.favoriteGames ?? [],
    following: data.following ?? [],
    followers: data.followers ?? [],
    pronouns: data.pronouns ?? 'they/them',
  }
}
