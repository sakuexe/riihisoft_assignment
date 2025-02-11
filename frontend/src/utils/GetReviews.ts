import type { Cat } from "./GetCats.ts";
import type { FetchError } from "./Fetch.d.ts";

// following the CatReviewDto record from the api
export type CatReview = {
  catReviewId: number,
  title: string,
  description: string,
  rating: number,
  createdAt: string
  cat: Cat
}

export type ReviewResponse = {
  reviews: CatReview[]
  error?: FetchError
}

// fetches the cats from the api and returns a response.
// includes the errors as values if they happened
export default async function getReviews(limit?: number): Promise<ReviewResponse> {
  const url = limit 
    ? `/api/reviews?limit=${limit}`
    : "/api/reviews";

  try {
    //throw new Error("Error for testing purposes")
    const response = await fetch(url, {
      method: "GET",
    })

    if (!response.ok) {
      return {
        reviews: [],
        error: {
          status: `${response.status}: ${response.statusText}`,
          message: await response.text()
        }
      };
    }

    const body: CatReview[] = await response.json();
    return { reviews: body };

  } catch (error: unknown) {
    console.error(error)
    return {
      reviews: [],
      error: {
        message: error instanceof Error ? `${error.message}` : `Unexpected error occured`,
      }
    };
  }
}
