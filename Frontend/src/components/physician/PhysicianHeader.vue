<template>

    <div
        class="
        relative
        overflow-hidden
        rounded-3xl
        bg-gradient-to-r
        from-[#0A3955]
        via-[#0D4B6E]
        to-[#3E82A8]
        px-8
        py-8
        shadow-lg">

        <!-- Decorative Circle -->

        <div
            class="
            absolute
            -right-20
            -top-20
            h-64
            w-64
            rounded-full
            bg-white/10">
        </div>

        <div
            class="
            absolute
            right-10
            bottom-0
            h-32
            w-32
            rounded-full
            bg-white/5">
        </div>

        <div class="relative z-10">

            <h1
                class="
                text-5xl
                font-extrabold
                tracking-tight
                text-white">

                Welcome, {{ physicianName }}

            </h1>

            <p
                class="
                mt-3
                text-lg
                text-white/90">

                Here's today's healthcare overview.

            </p>

        </div>

    </div>

</template>

<script setup>

import { computed } from "vue";

const physicianName = computed(() => {

    return sessionStorage.getItem("firstName") || "Doctor";

});

</script>