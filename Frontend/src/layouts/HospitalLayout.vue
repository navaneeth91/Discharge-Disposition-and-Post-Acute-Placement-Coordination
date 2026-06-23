<script setup>
import Sidebar from
'@/components/dashboard/Sidebar.vue'

import Navbar from
'@/components/dashboard/Navbar.vue'
</script>

<template>

<div class="flex">

    <Sidebar />

    <div class="flex-1">

        <Navbar />

        <main
            class="
            min-h-screen
            bg-[#F7F5FA]
            p-8">

            <slot />

        </main>

    </div>

</div>

</template>