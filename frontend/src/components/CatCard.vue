<script setup lang="ts">
defineProps<{
  reviewId: number
  catName: string
  title: string
  description: string
  rating: number
  createdAt: string
  image?: string
}>()
</script>

<template>
  <div class="card shadow-sharp" :style="{ viewTransitionName: `card-${reviewId}` }">

    <div class="card-header">
      <p><strong>{{ catName }}</strong></p>
      <div class="button"></div>
      <div class="button"></div>
    </div>

    <div class="card-image">
      <img :alt="`Image of the cat '${catName}'`" :src="image" />
    </div>

    <div class="card-body">
      <div>
        <p>{{ createdAt }}</p>
        <h4>{{ title }}</h4>
      </div>
      <p>
        {{ description }}
      </p>
    </div>

    <div class="card-footer">
      <div>
        <i v-for="_ in rating" class="fa-solid fa-star"></i>
        <i v-for="_ in 5 - rating" class="fa-regular fa-star"></i>
      </div>
    </div>
  </div>
</template>

<style scoped>
.card {
  --padding-inline: 1em;
  --padding-block: 0.75em;
  border: 4px solid var(--color-secondary);
  background-color: var(--color-accent);
  color: var(--color-secondary);
  display: flex;
  flex-direction: column;
}

.card * {
  margin: 0;
}

/* header */
.card-header {
  border-bottom: 4px solid var(--color-secondary);
  display: flex;
  gap: 1em;
  padding-inline: var(--padding-inline);
}

.card-header > p {
  margin-right: auto;
  font-weight: 700;
  font-size: 1.25em;
  padding-block: var(--padding-block);
}

.card-header > .button {
  width: 4px;
  background-color: var(--color-secondary);
}

/* image */
.card-image {
  border-bottom: 4px solid var(--color-secondary);
  overflow: clip;
  width: 100%;
  max-height: 225px;
}

.card-image>img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  object-position: center;
}

/* body */
.card-body {
  display: flex;
  flex-direction: column;
  gap: var(--padding-block);
  flex-grow: 1;
  padding: var(--padding-block) var(--padding-inline);
}

.card-body h4 {
  font-size: 1.25em;
}

.card-footer {
  border-top: 4px solid var(--color-secondary);
  padding: var(--padding-block) var(--padding-inline);
}

.card-footer > div:first-of-type {
  font-size: 1.5em;
}

</style>
