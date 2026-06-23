<script setup>
defineProps({
  loading: Boolean
})
</script>

<template>

  <button
    :disabled="loading"
    class="
w-full
rounded-xl
bg-[#614083]
py-3
font-medium
text-white
transition
duration-300
hover:-translate-y-1
hover:bg-[#53366F]
hover:shadow-xl
disabled:opacity-70">

    <span v-if="loading">
      Please wait...
    </span>

    <slot v-else />

  </button>

</template>