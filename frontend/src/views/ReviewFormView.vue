<script setup lang="ts">
import { onMounted, ref } from "vue"
import ReviewForm from "@/components/ReviewForm.vue";
import getCats from "@/utils/GetCats.ts";
import type { Cat } from "@/utils/GetCats.ts";
import type { FetchError } from "@/utils/Fetch.d.ts";

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
  <section class="heading">
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

<style scoped></style>
