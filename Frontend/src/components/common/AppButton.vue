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
    py-3
    font-semibold
    text-white
    transition-all
    duration-300
    hover:-translate-y-1
    hover:shadow-xl
    disabled:opacity-70"

    style="
    background: var(--primary);">

    <span v-if="loading">

        Please wait...

    </span>

    <slot v-else />

</button>

</template>

<style scoped>
button:hover {
    background: var(--primary-hover);
}
</style>