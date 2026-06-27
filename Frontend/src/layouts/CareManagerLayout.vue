<script setup>
import { computed } from 'vue'
import { RouterView } from 'vue-router'

import CareManagerSidebar from
'@/components/careManager/Sidebar.vue'

import Navbar from
'@/components/dashboard/Navbar.vue'

import { useUiStore }
from '@/stores/ui'

const ui = useUiStore()

const sidebarMargin =
    computed(() =>
        ui.sidebarCollapsed
            ? 'ml-24'
            : 'ml-72')
</script>

<template>

<div class="flex">

    <CareManagerSidebar />

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

            <RouterView />

        </main>

    </div>

</div>

</template>