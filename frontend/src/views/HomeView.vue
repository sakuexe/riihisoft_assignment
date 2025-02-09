<script setup lang="ts">
import CatCard from "@/components/CatCard.vue"
import { onMounted, ref } from "vue"

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
  const url = "http://localhost:5103/reviews?limit=3";
  const response = await fetch(url, {
    method: "GET",
  })
  const body: CatReview[] = await response.json();
  return body;
}
</script>

<template>
  <section class="heading">
    <h1>Purrfessional Balance</h1>
    <p><i>Take care of your mental health by consulting our purr-fessionals</i></p>
  </section>
  <section id="about">
    <div class="shadow-sharp">
      <div class="profile-pic">
        <img alt="Riihisoft logo" class="logo" src="@/assets/images/zazucat.webp" width="400" height="400" />
      </div>
      <div>
        <h3>What is this about?</h3>
        <p>
          As a busy fullstack .NET Core developer, your workday is likely packed with long coding sessions, deadlines,
          and complex projects that leave little room for self-care. That's where <strong>Purrfessional Balance</strong>
          steps in. 
        </p>
        <p>
          We offer a one-of-a-kind service that allows you to rent a therapy cat to bring peace, relaxation,
          and comfort to your workspace. Our carefully selected cats are trained to provide the kind of soothing
          presence that can help lower stress, improve mental clarity, and boost overall well-being during even the most
          hectic of coding sprints. With gentle purrs, soft snuggles, and calming energy, our therapy cats are here to
          help you recharge and regain focus.
        </p>
        <p>
          <strong>Dr Zazu </strong>- <i>CEO and Co-founder</i>
        </p>
      </div>
    </div>
  </section>
  <section>
    <p>
      Consult the very real reviews left by our <strong>many</strong> high paying customers and see if you
      there are any specific purr-fessionals for you. Each review highlights the unique traits of our furry
      professionals—from their purring style to their preferred snuggle techniques—so you can choose the cat that
      best matches your personality and work environment. With so many cats to choose from, you’ll find the perfect
      match to bring balance and tranquility to your coding routine.
    </p>
  </section>
  <section>
    <h2>Recent Reviews</h2>
    <div class="preview-cards">
      <CatCard v-for="review in catReviews" :key="review.CatReviewId" :catName="review.cat.name" :title="review.title"
        :description="review.description" :rating="review.rating" :createdAt="review.createdAt"
        :image="review.cat.imageUrl" />
    </div>
  </section>
</template>

<style scoped>
section {
  padding: 1em 2em;
}

section.heading {
  margin-top: 2em;
}

section.heading * {
  margin-block: 0;
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
