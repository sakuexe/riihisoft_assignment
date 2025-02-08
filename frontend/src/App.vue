<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <img alt="Riihisoft logo" class="logo" src="@/assets/images/riihisoft.svg" width="200" height="50" />
    <nav>
      <RouterLink to="/"><i class="fa-solid fa-house"></i> Etusivu</RouterLink>
      <RouterLink to="/about"><i class="fa-solid fa-house"></i> About</RouterLink>
      <RouterLink to="/forms"><i class="fa-solid fa-house"></i> Lomakkeet</RouterLink>
    </nav>
  </header>

  <main>
    <RouterView />
  </main>
</template>
