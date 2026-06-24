<script setup>
import { computed }
from 'vue'

import Sidebar
from '@/components/dashboard/Sidebar.vue'

import Navbar
from '@/components/dashboard/Navbar.vue'

import { useUiStore }
from '@/stores/ui'

const ui = useUiStore()

const sidebarMargin = computed(() =>
    ui.sidebarCollapsed
        ? 'ml-24'
        : 'ml-72'
)
</script>

<template>

<div class="flex">

    <Sidebar :insurance="true" />

    <div
        :class="[
            'flex-1',
            sidebarMargin
        ]">

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