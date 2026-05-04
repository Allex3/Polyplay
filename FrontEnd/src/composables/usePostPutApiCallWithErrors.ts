import { ref, type Ref } from 'vue'

export function usePostPutApiCallWithErrors() {
  // reset errors on new call so it's unique for each component that uses this composable
  const isInvalidFormat = ref(false)
  const errorText = ref('')
  const isPostedSuccessfully = ref(false)
  const successText = ref('')

  const validateInput = (
    apiResponse: any,
    successString: string = 'Saved Successfully',
  ): boolean => {
    if (apiResponse.success || apiResponse.errors === undefined) {
      // no errors but failure => no connection, act as added because cache {
      isInvalidFormat.value = false
      successText.value = successString
      isPostedSuccessfully.value = true
      return true
    }

    isPostedSuccessfully.value = false

    errorText.value = ''
    for (const [key, value] of Object.entries(apiResponse.errors)) {
      errorText.value += key + ': ' + value + '\n'

      // TODO HERE FORMAT ERRORS BETTER DEPENDING ON THE TPYE OF OBJECT
      // NOT JUST Name : VS GIBBERISH
    }

    isInvalidFormat.value = true

    return false
  }

  return { validateInput, isInvalidFormat, errorText, successText, isPostedSuccessfully }
}
