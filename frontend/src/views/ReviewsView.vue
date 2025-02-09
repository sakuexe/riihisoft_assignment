<script setup lang="ts">
import { ref, onMounted } from "vue"
import CatCard from "@/components/CatCard.vue"
// following the CatDto record from the api
type Cat = {
  catId: number
  name: string
  imageUrl?: string,
}

// following the CatReviewDto record from the api
type CatReview = {
  catReviewId: number,
  title: string,
  description: string,
  rating: number,
  createdAt: string
  cat: Cat
}

const catReviews = ref<CatReview[]>([]);
onMounted(async () => {
  catReviews.value = await getCatCards();
})

// TODO: try catch
async function getCatCards(): Promise<CatReview[]> {
  const url = "http://localhost:5103/reviews";
  const response = await fetch(url, {
    method: "GET",
  })
  const body: CatReview[] = await response.json();
  return body;
}
</script>

<template>
  <section class="about">
    <h1>The Reviews</h1>
    <p>
      Pick your favorite from our wide range of purrfessionals. These results are <i>100%</i> organic and authentic.
      So feel free to look around and pick one that speaks to you!
    </p>
  </section>

  <section>
    <div class="preview-cards">
      <CatCard 
        v-for="review in catReviews" 
        :key="review.CatReviewId"
        :catName="review.cat.name"
        :title="review.title"
        :description="review.description"
        :rating="review.rating"
        :createdAt="review.createdAt"
        :image="review.cat.imageUrl"
      />
    </div>
  </section>
</template>

<style>
section {
  padding: 1em 2em;
}

.about {
  margin-top: 2em;
}

.about > * {
  margin-block: 0;
}

.preview-cards {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 1em;
}
</style>
