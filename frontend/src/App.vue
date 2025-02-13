<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <div>
      <img alt="Riihisoft logo" class="logo" src="@/assets/images/riihisoft.svg" width="200" height="50" />
      <nav>
        <RouterLink to="/"><i class="fa-solid fa-house"></i> Home</RouterLink>
        <RouterLink to="/reviews"><i class="fa-solid fa-layer-group"></i> Reviews</RouterLink>
        <RouterLink to="/form"><i class="fa-solid fa-comment"></i> Leave a Review</RouterLink>
      </nav>
      <div class="credits">
        <p class="">Created by <a href="https://sakukarttunen.com" target="_blank">SakuK</a></p>
      </div>
    </div>
  </header>

  <main>
    <RouterView />
  </main>
</template>

<style scoped>
header > div {
  position: sticky;
  top: 2em;
  display: flex;
  flex-wrap: wrap;
  gap: 0.5em 1em;
  justify-content: space-between;
  align-items: center;

  @media (min-width: 1100px) {
    flex-direction: column;
    justify-content: start;
    align-items: start;
    gap: 1em;
  }
}

header .logo {
  width: clamp(100px, 100%, 200px);
  @media (min-width: 1100px) {
    margin-inline: auto;
  }
}

nav {
  display: flex;
  flex-wrap: wrap;
  gap: 0.5em;

  @media (min-width: 1100px) {
    flex-direction: column;
    align-items: start;
  }
}

nav>a {
  padding: 0.75em 1em;
  font-weight: 700;
  display: flex;
  gap: 0.5em;
  align-items: center;
  border: 4px solid var(--color-secondary);
  box-shadow: 10px 10px 0px var(--color-secondary);
  width: 100%;
  @media (min-width: 768px) {
    width: auto;
  }
  @media (min-width: 1100px) {
    width: 100%;
  }

  @media (prefers-reduced-motion: no-preference) {
    transition: all 0.15s var(--default-timing-func);
  }
}

nav>a:hover {
  background-color: var(--color-secondary);
  color: var(--color-primary);
}

header .credits {
  width: 100%;
  order: -1;
  text-align: end;
  @media (min-width: 1100px) {
    text-align: center;
    order: 0;
  }
}

header .credits > p {
  margin-block: 0;
  opacity: 75%;
}
</style>
