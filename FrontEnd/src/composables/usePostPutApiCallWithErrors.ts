import { ref, type Ref } from 'vue'

export function usePostPutApiCallWithErrors() {
  const validateInput = (
    apiResponse: any,
    isInvalidFormat: Ref,
    isSavedSuccessfully: Ref,
    errorText: Ref,
    successText: Ref,
    successString: string,
  ): boolean => {
    if (apiResponse.success || apiResponse.errors === undefined) {
      // no errors but failure => no connection, act as added because cache {
      isInvalidFormat.value = false
      successText.value = successString
      isSavedSuccessfully.value = true
      return true
    }

    isSavedSuccessfully.value = false

    errorText.value = ''
    for (const [key, value] of Object.entries(apiResponse.errors)) {
      errorText.value += key + ': ' + value + '\n'
    }

    isInvalidFormat.value = true

    return false
  }

  return { validateInput }
}
