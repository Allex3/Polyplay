import type { Game } from '@/data/Game'
import { cacheRequest } from './offlineApiSupport'
import type { User } from '@/data/User'

class UsersApi {
  constructor() {}

  private async callApi(method: string, endpoint: string, requestParams = {}) {
    const fetchData: { URL: string; options: any } = {
      URL: endpoint,
      options: {
        method: method,
        mode: 'cors',
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

  public async getUser(username: string, password: string) {
    let response = await this.callApi('GET', `/api/users/${username}`)
    if (response.errors !== undefined) return { success: false, errors: 'Username does not exist' }
    let user: User = response.usersData
    if (user.password != password) return { success: false, errors: 'Password is incorrect' }
    return { success: true, user: response.usersData }
  }

  public postUser(user: User) {
    const userWithoutID: object = (({ id, ...restOfUser }) => restOfUser)(user)
    return this.callApi('POST', '/api/users', { body: JSON.stringify(userWithoutID) })
  }

  public getUserRole(id: number) {
    return this.callApi('GET', `/api/users/${id}/roles`)
  }
}

const usersApi = new UsersApi()
export { usersApi as default, type UsersApi }
// NOTE: default export means that you can export that using any name, like import {games} from ..
