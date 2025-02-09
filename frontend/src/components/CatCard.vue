<script setup lang="ts">
defineProps<{
  catName: string
  title: string
  description: string
  rating: number
  createdAt: string
  image?: string
}>()
</script>

<template>
  <div class="card shadow-sharp">
    <div class="card-image">
      <img :alt="`Image of the cat '${catName}'`" :src="image" width="400" />
    </div>
    <div class="card-body">
      <div>
        <p><strong>{{ catName }}</strong></p>
        <p>{{ createdAt }}</p>
      </div>
      <h4>{{ title }}</h4>
      <p>
        {{ description }}
      </p>
    </div>
    <div class="card-footer">
      {{ rating }}
    </div>
  </div>
</template>

<style scoped>
.card {
  border: 4px solid black;
}
.card-image {
  border-bottom: 4px solid black;
  overflow: clip;
  width: 100%;
  aspect-ratio: 3/2;
}
.card-image > img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center;
}

.card-body > div:first-child {
  display: flex;
  flex-wrap: wrap;
  gap: 1em;
}
</style>
