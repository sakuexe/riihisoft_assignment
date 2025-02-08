<script setup lang="ts">
import CatCard from "@/components/CatCard.vue"
import { onMounted, ref, Ref } from "vue"

type CatReview = {
  catReviewId: number,
  title: string,
  description: string,
  imageUrl: string,
}

const catReviews = ref<CatReview[]>([]);
onMounted(async () => {
  catReviews.value = await getCatCards();
})

// TODO: try catch
async function getCatCards(): CatReview[] {
  const url = "http://localhost:5103/reviews";
  const response = await fetch(url, {
    method: "GET",
  })
  const body: CatReview[] = await response.json();
  return body;
}
</script>

<template>
  <section id="about">
    <h1>Kissakuvat.fi</h1>
    <div class="shadow-sharp">
      <div class="profile-pic">
        <img alt="Riihisoft logo" class="logo" src="@/assets/images/zazucat.webp" width="400" height="400" />
      </div>
      <div>
        <h3>Mistä kyse?</h3>
        <p><i>Anna äänesi kuulua</i></p>
        <p>
          Lorem ipsum dolor sit amet consectetur, adipisicing elit. Placeat corrupti aliquam dolore, aspernatur
          debitis incidunt corporis temporibus fugiat ipsam cum, reiciendis nihil fuga id. In quod beatae dolorem
          voluptatem temporibus.
        </p>
        <p>
          Lorem ipsum dolor sit amet consectetur, adipisicing elit. Placeat corrupti aliquam dolore, aspernatur
          debitis incidunt corporis temporibus fugiat ipsam cum, reiciendis nihil fuga id. In quod beatae dolorem
          voluptatem temporibus.
        </p>
      </div>
    </div>
  </section>
  <section>
    <h2>Uusimmat kuvat</h2>
    <div class="preview-cards">
      <CatCard v-for="review in catReviews" :key="review.CatReviewId" v-bind:title="review.title" v-bind:description="review.description" />
    </div>
  </section>
</template>

<style scoped>
section {
  padding: 1em 2em;
}

#about > div {
  display: grid;
  border: 4px solid black;
  background-color: #9399cc;;
  color: black;
  @media (min-width: 768px) {
    grid-template-columns: repeat(3, 1fr);
  }
}

#about > div > div:last-child {
  grid-column: 2/-1;
}

#about .profile-pic {
  overflow: clip;
  margin: 1em;
  border: 4px solid black;
  background-color: var(--color-background);
}

#about .profile-pic > img {
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
