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
    card-animation
    bg-white
    rounded-3xl
    p-6
    shadow-md
    hover:shadow-xl
    transition">

    <p
        class="
        text-sm
        text-slate-500">

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