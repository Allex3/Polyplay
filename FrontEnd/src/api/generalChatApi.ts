import type { Game } from '@/data/Game'
import { cacheRequest } from './offlineApiSupport'
import type { GeneralChatMessage } from '@/data/GeneralChatMessage'

class GeneralChatApi {
  generalChatWebSocket: undefined | WebSocket
  typingIndicatorsWebSocket: undefined | WebSocket
  typingTimeout: any
  isTyping: boolean

  constructor() {
    this.generalChatWebSocket = undefined
    this.typingIndicatorsWebSocket = undefined
    this.isTyping = false
    this.typingTimeout = null
  }

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

      return { success: true, chatData: result }
    } catch (error) {
      // actual connection down or user offline
      cacheRequest(fetchData)
      return {
        success: false,
        error: error instanceof Error ? error.message : error,
      }
    }
  }

  public async getMessages() {
    // 12 by default
    const response = await this.callApi('GET', `/api/generalChat`)
    if (response.chatData == undefined) return { messages: [] } // request didn't work
    return { messages: response.chatData }
  }

  public async sendMessage(message: GeneralChatMessage) {
    const messageWithoutId: object = (({ id, ...restOfMessage }) => restOfMessage)(message)
    console.log(messageWithoutId)
    this.generalChatWebSocket?.send(JSON.stringify(messageWithoutId))
  }

  public startChatConnection(updateView: Function, updateTypingIndicators: Function) {
    this.generalChatWebSocket = new WebSocket('https://172.30.248.197:5001/ws/generalChat')
    this.typingIndicatorsWebSocket = new WebSocket(
      'https://172.30.248.197:5001/ws/generalChatTypingIndicators',
    )

    this.generalChatWebSocket.onopen = function () {
      console.log('Connected to general chat')
    }
    this.generalChatWebSocket.onmessage = async function (event) {
      // received a message (broadcasted to all)
      const message: GeneralChatMessage = JSON.parse(event.data)
      console.log(message)
      await updateView(message) // update view with new message
    }
    this.generalChatWebSocket.onclose = function () {
      console.log('Disconnected from general chat')
    }

    this.typingIndicatorsWebSocket.onmessage = async function (event) {
      console.log(JSON.parse(event.data))
      const count: number = JSON.parse(event.data).count
      await updateTypingIndicators(count)
    }
  }

  public stopChatConnection() {
    this.generalChatWebSocket?.close() // close connection
  }

  public handleKeypress() {
    if (!this.isTyping) {
      this.isTyping = true
      this.typingIndicatorsWebSocket?.send(
        JSON.stringify({
          type: 'typing',
          room: 'general',
          active: true,
        }),
      )
    }
    clearTimeout(this.typingTimeout)
    this.typingTimeout = setTimeout(() => {
      this.isTyping = false
      this.typingIndicatorsWebSocket?.send(
        JSON.stringify({
          type: 'typing',
          room: 'general',
          active: false,
        }),
      )
    }, 4000)
  }
}

const generalChatApi = new GeneralChatApi()
export { generalChatApi as default, type GeneralChatApi }
// NOTE: default export means that you can export that using any name, like import {games} from ..
