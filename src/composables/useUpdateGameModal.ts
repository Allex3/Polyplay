import { ref } from 'vue'

const isUpdateGameModalOpen = ref(false)

export function useUpdateGameModal() {
  const openUpdateGameModal = () => (isUpdateGameModalOpen.value = true)
  const closeUpdateGameModal = () => (isUpdateGameModalOpen.value = false)
  return { isUpdateGameModalOpen, openUpdateGameModal, closeUpdateGameModal }
}
