<script setup lang="ts">
import { ref, onMounted } from "vue"
import CatCard from "@/components/CatCard.vue"
import getReviews from "../utils/GetReviews.ts"
import type { CatReview } from "../utils/GetReviews.ts"
import type { FetchError } from "../utils/Fetch.d.ts";

const catReviews = ref<CatReview[]>([]);
const fetchError = ref<FetchError | null>(null);

onMounted(async () => {
  fetchError.value = null;
  const reviewResponse = await getReviews();
  // and removed here
  if (reviewResponse.error) {
    fetchError.value = {
      message: reviewResponse.error.message,
      status: reviewResponse.error.status,
    };
    return;
  }
  catReviews.value = reviewResponse.reviews;
})
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
    <div v-if="!fetchError" class="preview-cards">
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

    <div v-else class="error-msg shadow-sharp">
      <p>Error fetching the reviews</p>
      <p v-if="fetchError.status">{{ fetchError.status }}</p>
      <p v-if="fetchError.message">{{ fetchError.message }}</p>
    </div>
  </section>
</template>

<style>
.about {
  max-width: 70ch;
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
