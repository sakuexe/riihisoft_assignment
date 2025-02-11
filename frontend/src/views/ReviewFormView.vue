<script setup lang="ts">
import { onMounted, ref } from "vue"
import ReviewForm from "@/components/ReviewForm.vue";
import getCats from "../utils/GetCats.ts";
import type { Cat, FetchError } from "../utils/GetCats.ts";

const cats = ref<Cat[]>([]);
const fetchError = ref<FetchError | null>(null);

onMounted(async () => {
  // a loader state could be added here
  const catResponse = await getCats();
  // and removed here
  if (catResponse.error) {
    fetchError.value = {
      message: catResponse.error.message,
      status: catResponse.error.status,
    }
    return;
  }
  cats.value = catResponse.cats;
})

</script>

<template>
  <section class="about">
    <p>Let us know how it went</p>
    <h1>Leave your review</h1>
  </section>
  <section>

    <div v-if="fetchError" class="error-msg shadow-sharp">
      <p>Error fetching the cats from the database</p>
      <p v-if="fetchError.status">{{ fetchError.status }}</p>
      <p v-if="fetchError.message">{{ fetchError.message }}</p>
    </div>

    <ReviewForm :cats="cats" />
  </section>
</template>

<style scoped>
section {
  padding: 1em 2em;
}

section:has(.error-msg) {
  display: grid;
  gap: 1em;
}

.about {
  margin-top: 2em;
}

.about>* {
  margin-block: 0;
}
</style>
