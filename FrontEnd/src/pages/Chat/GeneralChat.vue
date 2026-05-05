<script setup lang="ts">
import apiService from '@/api/apiService'
import { useShowProfileAndHideLogin } from '@/composables/useShowProfileAndHideLogin'
import { createGeneralChatMessage, type GeneralChatMessage } from '@/data/GeneralChatMessage'
import { useUserStore } from '@/stores/userStore'
import { onUnmounted, ref } from 'vue'

apiService.generalChat.startChatConnection(updateChatView)

const messages = ref<GeneralChatMessage[]>((await apiService.generalChat.getMessages()).messages)

const userStore = useUserStore()

const messageToSend = ref<GeneralChatMessage>(
  createGeneralChatMessage({ userId: userStore.user.id }),
)

const { isUserLoggedIn } = useShowProfileAndHideLogin()

function postMessage(): void {
  if (messageToSend.value.message.length == 0) return
  apiService.generalChat.sendMessage(messageToSend.value)
  messageToSend.value.message = ''
}

function updateChatView(newMessage: GeneralChatMessage): void {
  messages.value.push(newMessage)
}

onUnmounted(() => {
  apiService.generalChat.stopChatConnection()
})
</script>
<template>
  <div class="retro-window w-2/3 h-2/3 m-auto p-4">
    <div class="overflow-y-scroll flex flex-col gap-2">
      <div class="flex flex-row gap-2 wrap-break-word" v-for="message in messages">
        <span>{{ message.userId }}:</span>
        <span>{{ message.message }}</span>
      </div>
      <div v-show="isUserLoggedIn" class="flex flex-col items-center">
        <form @submit.prevent="postMessage" class="flex flex-row w-full">
          <input
            type="textarea"
            v-model="messageToSend.message"
            name="messageInput"
            placeholder="Message"
            class="comic-neue flex-1 resize-none overflow-hidden wrap-break-word border-2 p-2 bg-[#bfedef]"
          />
          <input
            type="submit"
            name="sendMessageInput"
            class="p-2 comic-neue-bold border-2 border-l-0 bg-[#f4b5ea] hover:cursor-pointer hover:bg-[#e4a6d9]"
            value="Send"
          />
        </form>
      </div>
    </div>
  </div>
</template>
