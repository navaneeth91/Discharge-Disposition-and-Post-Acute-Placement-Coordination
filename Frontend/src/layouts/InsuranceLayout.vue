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
                min-h-screen
                p-8"
                style="
                background:
                var(--background);">

                <slot />

            </main>

        </div>

    </div>

</template>