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
