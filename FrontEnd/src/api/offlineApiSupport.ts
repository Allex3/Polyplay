export function cacheRequest(fetchData: { URL: string; options: any }) {
  if (fetchData.options.method == 'GET') return

  fetchData.options.body = JSON.parse(fetchData.options.body) // it's stringified, not good
  // since it will be stringified anyway again
  var cachedRequests: object[] = JSON.parse(localStorage.getItem('cachedApiRequests') ?? '[]')
  cachedRequests.push(fetchData)
  localStorage.setItem('cachedApiRequests', JSON.stringify(cachedRequests))
}

const url = 'https://172.30.248.197:5001/api/games'

async function connectionEstablished(): Promise<boolean> {
  try {
    const result = await fetch(url, {
      method: 'GET',
      mode: 'no-cors', // Suppresses CORS errors but returns 'opaque' response type
      cache: 'no-cache',
      referrerPolicy: 'no-referrer',
    })
    return true
  } catch (error) {
    // actual connection down or user offline
    return false
  }
}

// this function is called every 10 seconds
// and is used to call the api requests that are stored as objects in localStorage

//NOTE: this is useless for GET requests, because if I am not on the page i called it
// then even if I call it again, it won't do anything with the result
// this would only be useful IF I cache the response somewhere, which I don't, think about this later
// even if I somehow tie this function to the component, or make a unique function there
// because if that component is not into view,
//  it will refresh the API anyway when going on that page thus calling it yet again
export async function fetchCachedRequests() {
  var cachedRequests: { URL: string; options: object }[] = JSON.parse(
    localStorage.getItem('cachedApiRequests') ?? '[]',
  )
  if (cachedRequests.length == 0) return // do nothing, no requests

  if (!(await connectionEstablished())) {
    // client/server offline
    return
  }

  cachedRequests.forEach(async (httpRequest: { URL: string; options: any }) => {
    // it stringifies when trying the request first, then when parsed it's not
    httpRequest.options.body = JSON.stringify(httpRequest.options.body)
    const resposne = await fetch(httpRequest.URL, httpRequest.options)
  })
  localStorage.removeItem('cachedApiRequests')
}
