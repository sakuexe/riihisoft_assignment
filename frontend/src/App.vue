<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <img alt="Riihisoft logo" class="logo" src="@/assets/images/riihisoft.svg" width="200" height="50" />
    <nav>
      <RouterLink to="/"><i class="fa-solid fa-house"></i> Home</RouterLink>
      <RouterLink to="/reviews"><i class="fa-solid fa-house"></i> Reviews</RouterLink>
      <RouterLink to="/form"><i class="fa-solid fa-house"></i> Leave a Review</RouterLink>
    </nav>
  </header>

  <main>
    <RouterView />
  </main>
</template>
