export type GeneralChatMessage = {
  id: string // string Id because... mongodb
  userId: number
  username: string
  message: string
}

export function createGeneralChatMessage(
  data: Partial<GeneralChatMessage> = {},
): GeneralChatMessage {
  return {
    id: data.id ?? '',
    username: data.username ?? '',
    userId: data.userId ?? -1,
    message: data.message ?? '',
  }
}
