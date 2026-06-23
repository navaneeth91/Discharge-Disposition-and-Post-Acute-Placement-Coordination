<script setup>
import Sidebar from '@/components/dashboard/Sidebar.vue'
import Navbar from '@/components/dashboard/Navbar.vue'
</script>

<template>

    <div class="flex">

        <Sidebar :insurance="true" />

        <div class="flex-1">

            <Navbar />

            <main
                class="
                p-8
                bg-[#F7F5FA]
                min-h-screen">

                <slot />

            </main>

        </div>

    </div>

</template>