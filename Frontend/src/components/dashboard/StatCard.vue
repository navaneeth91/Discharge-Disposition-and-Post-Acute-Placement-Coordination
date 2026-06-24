<script setup>
defineProps({
    title: String,
    value: Number,
    color: String
})
</script>

<template>

<div
    class="
    rounded-3xl
    p-6
    transition-all
    duration-300
    hover:-translate-y-1"
    style="
    background:white;
    box-shadow:var(--card-shadow);">

    <p
        class="
        text-sm
        font-medium"
        style="
        color:
        var(--text-secondary);">

        {{ title }}

    </p>

    <h2
        class="
        text-4xl
        font-bold
        mt-4"
        :style="{ color }">

        {{ value }}

    </h2>

</div>

</template>