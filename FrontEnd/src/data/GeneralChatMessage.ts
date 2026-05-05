export type GeneralChatMessage = {
  id: string // string Id because... mongodb
  userId: number
  message: string
}

export function createGeneralChatMessage(
  data: Partial<GeneralChatMessage> = {},
): GeneralChatMessage {
  return {
    id: data.id ?? '',
    userId: data.userId ?? -1,
    message: data.message ?? '',
  }
}
