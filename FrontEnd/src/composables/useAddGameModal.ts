import { ref } from 'vue'

const isAddGameModalOpen = ref(false)

export function useAddGameModal() {
  const openAddGameModal = () => (isAddGameModalOpen.value = true)
  const closeAddGameModal = () => (isAddGameModalOpen.value = false)
  return { isAddGameModalOpen, openAddGameModal, closeAddGameModal }
}
