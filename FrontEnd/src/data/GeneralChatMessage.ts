export type GeneralChatMessage = {
  id: string // string Id because... mongodb
  userName: string
  message: string
}

export function createGeneralChatMessage(
  data: Partial<GeneralChatMessage> = {},
): GeneralChatMessage {
  return {
    id: data.id ?? '',
    userName: data.userName ?? '',
    message: data.message ?? '',
  }
}
