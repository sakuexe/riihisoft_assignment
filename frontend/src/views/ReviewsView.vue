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

#about>div {
  display: grid;
  border: 4px solid black;
  background-color: #9399cc;
  color: black;

  @media (min-width: 768px) {
    grid-template-columns: repeat(3, 1fr);
  }
}

#about>div>div:last-child {
  grid-column: 2/-1;
}

#about .profile-pic {
  overflow: clip;
  margin: 1em;
  border: 4px solid black;
  background-color: var(--color-background);
}

#about .profile-pic>img {
  width: 100%;
  height: 100%;
  object-position: center;
  object-fit: cover;
}

.preview-cards {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 1em;
}
</style>
