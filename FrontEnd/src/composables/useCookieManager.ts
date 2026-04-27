export function useCookieManager() {
  const readCookie = <T>(cookieName: string) => {
    const cookies = document.cookie.split('; ')

    for (const cookie of cookies) {
      const [key, value] = cookie.split('=')

      if (key === cookieName && value !== undefined) return JSON.parse(value) as T
    }

    return null
  }
  const writeCookie = <T>(cookieName: string, cookieValue: T, numberOfDays: number) => {
    const cookieExpiryDate = new Date()
    cookieExpiryDate.setDate(cookieExpiryDate.getDate() + numberOfDays * 24 * 60 * 60)

    const cookieValueString = JSON.stringify(cookieValue)
    const cookieExpiryDateUTCString = cookieExpiryDate.toUTCString()

    document.cookie = `${cookieName}=${cookieValueString};expires=${cookieExpiryDateUTCString};path=/`
  }
  return { readCookie, writeCookie }
}
