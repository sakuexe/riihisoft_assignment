<script setup lang="ts">
import { ref } from "vue"
import { useRouter } from 'vue-router';
import type { Cat } from "@/utils/GetCats";

export type FormError = {
  message: string,
  status?: string,
  error?: string
}

const props = defineProps<{
  cats: Cat[]
}>();

const router = useRouter();

const formError = ref<FormError | null>(null);
const currentCat = ref<Cat | null>(null);

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
        status: `${response.status}: ${response.statusText}`,
        message: await response.text(),
      }
      return;
    }
  } catch (error) {
    formError.value = {
      message: "Error while connecting to the backend",
      error: error instanceof Error ? `${error.message}` : `Unexpected error occured`,
    }
    return;
  }

  router.push({ name: 'reviews' });
  return;
}

const currentRating = ref<number>(3);
const maxRating = 5;

function onRatingChange(newRating: number) {
  currentRating.value = newRating;
}

const changeCurrentCat = (event: Event) => {
  const element = event.currentTarget as HTMLSelectElement | null;
  if (!element) {
    throw new Error("Select element could not be fetched from event")
  }

  const changedCat = props.cats.find(cat => cat.catId === parseInt(element.value));
  if (!changedCat) {
    currentCat.value = null;
    return;
  }

  currentCat.value = changedCat;
}
</script>

<template>
  <div class="shadow-sharp">
    <div class="cat-image">
      <img v-if="currentCat" :src="currentCat.imageUrl" :alt="`preview image for ${currentCat.name}`" />
      <img v-if="!currentCat" src="@/assets/images/cat-customer-service.svg" alt="default icon when no cat is chosen"
        class="placeholder" />
    </div>

    <form action="/api/reviews" method="POST" @submit="createReview">

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
        <label for="review">Your review:</label>
        <textarea name="review" id="review" rows="5" placeholder="My experience with it was..."></textarea>
      </div>

      <div>
        <label for="rating">Rating:</label>
        <input type="number" id="rating" name="rating" :value="currentRating">
        <div>
          <button v-for="star in currentRating" :for="`rating-${star}`" type="button"
            :aria-label="`rating of ${currentRating} of ${maxRating}`" @click="() => onRatingChange(star)">
            <i class="fa-solid fa-star"></i>
          </button>

          <button v-if="currentRating < maxRating" v-for="star in maxRating - currentRating"
            :for="`rating-${currentRating + star}`" type="button"
            :aria-label="`rating of ${currentRating + star} of ${maxRating}`"
            @click="() => onRatingChange(currentRating + star)">
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
      <p v-if="formError.status">{{ formError.status }}</p>
      <p v-if="formError.message">{{ formError.message }}</p>
      <p v-if="formError.error">{{ formError.error }}</p>
    </div>

  </div>
</template>

<style scoped>
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

form>div:not(:has(button[type="submit"])):focus-within {
  background-color: var(--color-secondary);
}

form>div:not(:has(button[type="submit"])) {
  display: grid;
  padding: 0.5em 1em;
}

form>div:not(:last-of-type) {
  border-bottom: 4px solid var(--color-secondary);
}

form input,
form textarea,
form select {
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

/* cat picker */
form select {
  appearance: none;
}

form>div:has(select) {
  position: relative;
}

/* because there is appearance: none, we need to add 
a custom dropdown indicator. (a simple triangle in this case) */
form>div:has(select)::after {
  --arrow-size: 10px;
  content: "";
  position: absolute;
  inset: 0;
  margin: auto 2em 1.125em auto;
  width: 0;
  height: 0;
  border-left: var(--arrow-size) solid transparent;
  border-right: var(--arrow-size) solid transparent;
  border-top: calc(var(--arrow-size) * 0.75) solid var(--color-primary);
  pointer-events: none;
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

form>div:has(button[type="submit"])>button:hover,
form>div:has(button[type="submit"])>button:focus {
  background-color: var(--color-secondary);
}

/* error messages */
form ~ .error-msg {
  border: none;
  border-top: 4px solid var(--color-secondary);
  @media (min-width: 1100px) {
    grid-column: 1/-1;
  }
}
</style>
