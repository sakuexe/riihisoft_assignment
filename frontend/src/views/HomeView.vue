<script setup lang="ts">
import CatCard from "@/components/CatCard.vue"
import { onMounted, ref } from "vue"
import getReviews from "../utils/GetReviews.ts"
import type { CatReview } from "../utils/GetReviews.ts"
import type { FetchError } from "../utils/Fetch.d.ts";

const catReviews = ref<CatReview[]>([]);
const fetchError = ref<FetchError | null>(null);

onMounted(async () => {
  fetchError.value = null;
  const reviewResponse = await getReviews(3);
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

  <section id="about">
    <div class="heading">
      <h1>Purrfessional Balance</h1>
      <p><i>Take care of your mental health by consulting our purr-fessionals</i></p>
    </div>
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
          - <strong>Dr Zazu </strong> <i>CEO and Co-founder</i>
        </p>
      </div>
    </div>

    <div>
      <h3>How does it work?</h3>
      <p>
        Consult the very real reviews left by our <strong>many</strong> high paying customers and see if you
        there are any specific purr-fessionals for you. Each review highlights the unique traits of our furry
        professionals—from their purring style to their preferred snuggle techniques—so you can choose the cat that
        best matches your personality and work environment. With so many cats to choose from, you’ll find the perfect
        match to bring balance and tranquility to your coding routine.
      </p>
    </div>
  </section>

  <section>
    <h2>Recent Reviews</h2>
    <div v-if="!fetchError" class="preview-cards">
      <CatCard v-for="review in catReviews" :key="review.CatReviewId" :reviewId="review.catReviewId"
        :catName="review.cat.name" :title="review.title" :description="review.description" :rating="review.rating"
        :createdAt="review.createdAt" :image="review.cat.imageUrl" />
    </div>

    <div v-if="fetchError" class="error-msg shadow-sharp">
      <p>Error fetching the reviews</p>
      <p v-if="fetchError.status">{{ fetchError.status }}</p>
      <p v-if="fetchError.message">{{ fetchError.message }}</p>
    </div>

  </section>
</template>

<style scoped>
.heading {
  grid-column: 1/-1;
}

#about {
  display: grid;
  gap: 0.75em 1.5em;

  @media (min-width: 1500px) {
    grid-template-columns: 2fr 1fr;
  }
}

#about>div.shadow-sharp {
  display: grid;
  border: 4px solid black;
  background-color: #9399cc;
  color: black;

  @media (min-width: 768px) {
    grid-template-columns: repeat(3, 1fr);
  }
}

#about>div.shadow-sharp>div:not(.profile-pic) {
  padding: 1em 1em;
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
