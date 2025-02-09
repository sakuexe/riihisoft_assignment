<script setup lang="ts">
import { onMounted, ref } from "vue"
import { useRouter } from 'vue-router';

const router = useRouter();

// following the CatDto record from the api
type Cat = {
  catId: number
  name: string
  imageUrl?: string,
}

const cats = ref<Cat[]>([]);
const currentCat = ref<Cat | null>(null);

const changeCurrentCat = (event: Event) => {

  const element = event.currentTarget as HTMLSelectElement | null;
  if (!element) {
    throw new Error("Select element could not be fetched from event")
  }

  const changedCat = cats.value.find(cat => cat.catId === parseInt(element.value));
  if (!changedCat) {
    currentCat.value = null;
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

type ServerError = {
  message: string,
  statusCode?: string,
  error?: Error
}

const formError = ref<ServerError | null>(null);

async function createReview(event: SubmitEvent) {
  event.preventDefault();
  formError.value = null;

  const form = event.target as HTMLFormElement

  try {
    const response = await fetch(form.action, {
      method: form.method,
      body: new FormData(form)
    })

    if (!response.ok) {
      formError.value = {
        statusCode: `${response.status}: ${response.statusText}`,
        message: await response.text(),
      }
      return;
    }
  } catch (error) {
    formError.value = {
      message: "Error while connecting to the backend",
      error: error,
    }
    return;
  }

  router.push({ name: 'reviews' });
  return;
}

const currentRating = ref<number>(3);
const maxRating = 5;

function onRatingChange(event: Event, newRating: number) {
  currentRating.value = newRating;
}
</script>

<template>
  <section class="about">
    <p>Let us know how it went</p>
    <h1>Leave your review</h1>
  </section>
  <section>
    <div class="shadow-sharp">
      <div class="cat-image">
        <img v-if="currentCat" :src="currentCat.imageUrl" :alt="`preview image for ${currentCat.value}`" />
        <img v-if="!currentCat" src="@/assets/images/cat-customer-service.svg" alt="default icon when no cat is chosen"
          class="placeholder" />
      </div>
      <form action="http://localhost:5103/reviews" method="POST" @submit="createReview">

        <div>
          <label for="cat-id">Your therapist:</label>
          <select name="cat-id" id="cat-id" @change="changeCurrentCat">
            <option value="">--Please choose a cat--</option>
            <option v-for="cat in cats" :value="cat.catId">{{ cat.name }}</option>
          </select>
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
          <label for="rating">Rating:</label>
          <input type="number" id="rating" name="rating" :value="currentRating">
          <div>
            <button v-for="star in currentRating" :for="`rating-${star}`" type="button"
              @click="(event) => onRatingChange(event, star)">
              <i class="fa-solid fa-star"></i>
            </button>

            <button v-if="currentRating < maxRating" v-for="star in maxRating - currentRating"
              :for="`rating-${currentRating + star}`" type="button"
              @click="(event) => onRatingChange(event, currentRating + star)">
              <i class="fa-regular fa-star"></i>
            </button>
          </div>
        </div>

        <div>
          <button type="submit">Submit</button>
          <button type="reset">Clear</button>
        </div>

      </form>

      <div v-if="formError != null" class="error-msg">
        <p v-if="formError.statusCode">{{ formError.statusCode }}</p>
        <p v-if="formError.message">{{ formError.message }}</p>
        <p v-if="formError.error">{{ formError.error }}</p>
      </div>

    </div>
  </section>
</template>

<style scoped>
section {
  padding: 1em 2em;
}

.about {
  margin-top: 2em;
}

.about>* {
  margin-block: 0;
}

section:has(form)>div {
  display: grid;
  border: 4px solid var(--color-secondary);
  background-color: color-mix(in srgb, var(--color-background), #fff 25%);

  @media (min-width: 1100px) {
    grid-template-columns: repeat(2, 1fr);
  }
}

.cat-image {
  width: 100%;
  aspect-ratio: 3/2;
  overflow: clip;
  background-color: var(--color-accent);

  @media (min-width: 1100px) {
    contain: size;
    /* do not grow the parent element */
    aspect-ratio: auto;
  }
}

.cat-image img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center;
  z-index: -1;
}

.cat-image img.placeholder {
  object-fit: contain;
  width: 50%;
  inset: 0;
  margin: auto;
}

form {
  display: grid;
  border-top: 4px solid var(--color-secondary);

  @media (min-width: 1100px) {
    border-top: none;
    border-left: 4px solid var(--color-secondary);
  }
}

form>div:not(:has(button[type="submit"])) {
  display: grid;
  padding: 0.5em 1em;
}

form>div:not(:last-of-type) {
  border-bottom: 4px solid var(--color-secondary);
}

form input,
form textarea {
  background-color: transparent;
  outline: 0;
  border: 0;
  color: currentColor;
  font-family: monospace;
  font-size: 1em;
}

form label {
  font-size: 0.8em;
}


/* rating */
form input[name="rating"] {
  display: none;
}

form div:has(input[name="rating"]) button {
  background-color: transparent;
  border: none;
  font-size: 2em;
  cursor: pointer;
  color: currentColor;
}

/* buttons */
form>div:has(button[type="submit"]) {
  display: flex;
  flex-wrap: wrap;
}

form>div:has(button[type="submit"])>button {
  flex-grow: 1;
  padding: 0.5em 1em;
  background-color: transparent;
  border: none;
  color: currentColor;

  @media (prefers-reduced-motion: no-preference) {
    transition: all 0.15s var(--default-timing-func);
  }
}

form>div:has(button[type="submit"])>button:first-of-type {
  border-right: 4px solid var(--color-secondary);
}

form>div:has(button[type="submit"])>button:hover {
  background-color: var(--color-secondary);
}

/* error messages */
.error-msg {
  border-top: 4px solid var(--color-secondary);
  padding: 0.5em 1em;
  background-color: #703d39;

  @media (min-width: 1100px) {
    grid-column: 1/-1;
  }
}

.error-msg>* {
  margin-block: 0;
}

.error-msg>p:first-child {
  font-weight: 700;
  font-size: 1.5em;
  /*color: #c7726b;*/
}
</style>
