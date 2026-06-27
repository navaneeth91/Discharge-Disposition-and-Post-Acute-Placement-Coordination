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
    shadow-xl"

    style="
    background:
    linear-gradient(
        135deg,
        #003049,
        #0D4A6B,
        #669BBC
    );">

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
        text-blue-100">

        Review incoming referrals and
        manage placement requests efficiently.

    </p>

</div>

</template>