import type { Game } from '@/data/Game'
import { cacheRequest } from './offlineApiSupport'
import type { RegisterUser, User } from '@/data/User'
import { BASE_URL } from './apiService'

class UsersApi {
  constructor() {}

  private async callApi(method: string, endpoint: string, requestParams = {}) {
    const fetchData: { URL: string; options: any } = {
      URL: BASE_URL + endpoint,
      options: {
        method: method,
        headers: {
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        ...requestParams, // put the requestParams object: body, etc.
      }, //TODO after POST return websocket
    }

    try {
      const response = await fetch(fetchData.URL, fetchData.options)

      if (!response.ok) {
        return { success: false, errors: await response.json() } //if NOT ok, we get the POST/PUT data validation errors
      }

      const result = await response.json()

      return { success: true, usersData: result }
    } catch (error) {
      // actual connection down or user offline
      cacheRequest(fetchData)
      return {
        success: false,
        error: error instanceof Error ? error.message : error,
      }
    }
  }

  public async loginUser(username: string, password: string) {
    return this.callApi('POST', '/api/authentication/login', {
      body: JSON.stringify({ userName: username, password: password }),
    })
  }

  public registerUser(user: RegisterUser) {
    return this.callApi('POST', '/api/authentication/register', {
      body: JSON.stringify(user),
    })
  }

  public getUserRoles(username: string) {
    return this.callApi('GET', `/api/authentication/${username}/roles`)
  }
}

const usersApi = new UsersApi()
export { usersApi as default, type UsersApi }
// NOTE: default export means that you can export that using any name, like import {games} from ..
