// following the CatDto record from the api
export type Cat = {
  catId: number
  name: string
  imageUrl?: string,
}

export type FetchError = {
  message: string,
  status?: string,
}

export type CatResponse = {
  cats: Cat[],
  error?: FetchError
}

// fetches the cats from the api and returns a response.
// includes the errors as values if they happened
export default async function getCats(): Promise<CatResponse> {
  const url = `/api/cats`;

  try {
    const response = await fetch(url, {
      method: "GET",
    })

    if (!response.ok) {
      return {
        cats: [],
        error: {
          status: `${response.status}: ${response.statusText}`,
          message: await response.text()
        }
      };
    }

    const body: Cat[] = await response.json();
    return { cats: body };

  } catch (error: unknown) {
    console.error(error)
    return {
      cats: [],
      error: {
        message: `Could not connect to the database`,
      }
    };
  }
}
