<script setup lang="ts">
import { onMounted, ref } from "vue"

// following the CatDto record from the api
type Cat = {
  catId: number
  name: string
  imageUrl?: string,
}

const cats = ref<Cat[]>([]);
const currentCat = ref<Cat>();

const changeCurrentCat = (event: Event) => {
  const element = event.currentTarget as HTMLSelectElement | null;
  if (!element) {
    throw new Error("Select element could not be fetched from event")
  }
  const changedCat = cats.value.find(cat => cat.catId === parseInt(element.value));
  if (!changedCat) {
    return;
  }

  currentCat.value = changedCat;
}

onMounted(async () => {
  cats.value = await getCats();
})

// TODO: try catch
async function getCats(): Promise<Cat[]> {
  const url = "http://localhost:5103/cats";
  const response = await fetch(url, {
    method: "GET",
  })
  const body: Cat[] = await response.json();
  return body;
}
</script>

<template>
  <section>
    <p>Let us know how it went</p>
    <h1>Leave your review</h1>
  </section>
  <section>
    <form action="http://localhost:5103/reviews" method="POST">
      <div class="cat-image">
        <img v-if="currentCat != null" :src="currentCat.imageUrl" :alt="`preview image for ${currentCat.value}`" />
      </div>

      <div>
        <label for="title">Title:</label>
        <input type="text" name="title" id="title" rows="5" placeholder="What a kitty!"></input>
      </div>

      <div>
        <label for="cat">Choose a pet:</label>
        <textarea name="review" id="review" rows="5" placeholder="My experience with it was..."></textarea>
      </div>

      <div>
        <label for="cat-id">Choose a pet:</label>
        <select name="cat-id" id="cat-id" @change="changeCurrentCat">
          <option value="">--Please choose a cat--</option>
          <option v-for="cat in cats" :value="cat.catId">{{ cat.name }}</option>
        </select>
      </div>

      <div>
        <p>Rating</p>
        <div v-for="rating in 5">
          <input type="radio" :id="`rating-${rating}`" name="rating" :value="rating">
          <label :for="`rating-${rating}`">{{ "⭐ ".repeat(rating) }}</label>
        </div>
      </div>
      <button type="submit">Submit</button>
    </form>
  </section>
</template>

<style scoped>
form {
  display: grid;
}

.cat-image:not(:empty) {
  width: 100%;
  aspect-ratio: 3/2;
  overflow: clip;
}

img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center;
}
</style>
