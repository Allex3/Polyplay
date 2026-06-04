import { USER_ROLE } from './Roles'

export type User = {
  userName: string
  pronouns: string
  roles: number[]
}

export function createUser(data: Partial<User> = {}): User {
  return {
    userName: data.userName ?? '',
    pronouns: data.pronouns ?? 'they/them',
    roles: data.roles ?? [USER_ROLE.USER],
  }
}

export type RegisterUser = {
  userName: string
  email: string
  password: string
  wantsToReceiveGameMails: boolean
}

export function createRegisterUser(data: Partial<RegisterUser> = {}): RegisterUser {
  return {
    userName: data.userName ?? '',
    email: data.email ?? '',
    password: data.password ?? '',
    wantsToReceiveGameMails: data.wantsToReceiveGameMails ?? false,
  }
}

export type JwtToken = {
  token: string
  refreshToken: string
  expiresAt: Date
}

export function createJwtToken(data: Partial<JwtToken> = {}): JwtToken {
  return {
    token: data.token ?? '',
    refreshToken: data.token ?? '',
    expiresAt: data.expiresAt ?? new Date(1989, 1, 1, 1, 1),
  }
}

export type TokenRequest = {
  token: string
  refreshToken: string
}

export function createTokenRequest(data: Partial<TokenRequest> = {}): TokenRequest {
  return {
    token: data.token ?? '',
    refreshToken: data.refreshToken ?? '',
  }
}
