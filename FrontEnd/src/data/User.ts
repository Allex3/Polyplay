export type User = {
  id: number
  username: string
  email: string
  password: string
  wantsNewsletter: boolean
  favoriteGames: number[]
  following: string[]
  followers: string[]
  pronouns: string
  roles: number[]
}

export function createUser(data: Partial<User> = {}): User {
  return {
    id: data.id ?? -1,
    username: data.username ?? '',
    email: data.email ?? '',
    password: data.password ?? '',
    wantsNewsletter: data.wantsNewsletter ?? false,
    favoriteGames: data.favoriteGames ?? [],
    following: data.following ?? [],
    followers: data.followers ?? [],
    pronouns: data.pronouns ?? 'they/them',
    roles: data.roles ?? [1], // just user
  }
}
