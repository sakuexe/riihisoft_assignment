<script setup>
</script>

<template>
  <div class="card shadow-sharp">
    <div class="card-image">
      <img alt="Image of the cat 'maka'" src="@/assets/images/cats/uni.jpg" width="400" />
    </div>
    <div class="card-body">
      <h4>Kissa</h4>
      <p>
        Lorem, ipsum dolor sit amet consectetur adipisicing elit. Sint vero aliquam velit nostrum quam, architecto
        commodi dolor consequatur vel tempora.
      </p>
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
</style>
