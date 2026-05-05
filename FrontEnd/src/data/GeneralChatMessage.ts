export type GeneralChatMessage = {
  id: number
  userId: number
  message: string
}

export function createGeneralChatMessage(
  data: Partial<GeneralChatMessage> = {},
): GeneralChatMessage {
  return {
    id: data.id ?? -1,
    userId: data.userId ?? -1,
    message: data.message ?? '',
  }
}
