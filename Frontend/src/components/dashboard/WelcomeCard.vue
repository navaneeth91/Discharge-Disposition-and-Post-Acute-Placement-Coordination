<script setup>
import { useAuthStore }
from '@/stores/auth'

const auth =
    useAuthStore()
</script>

<template>

<div
    class="
    rounded-3xl
    p-8
    text-white
    bg-gradient-to-r
    from-[#614083]
    to-[#9470B0]
    shadow-xl">

    <h1
        class="
        text-4xl
        font-bold">

        Welcome,
        {{ auth.firstName }}

    </h1>

    <p
        class="
        mt-3
        text-purple-100">

        Here's today's healthcare overview.

    </p>

</div>

</template>