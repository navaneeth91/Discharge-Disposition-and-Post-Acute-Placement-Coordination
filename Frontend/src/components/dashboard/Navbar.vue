<script setup>
import {
    Bell,
    Menu,
    ChevronDown
}
from 'lucide-vue-next'

import { ref }
from 'vue'

import { useUiStore }
from '@/stores/ui'

import { useAuthStore }
from '@/stores/auth'

const ui = useUiStore()

const auth = useAuthStore()

const dropdown = ref(false)
</script>

<template>

<header
    class="
    h-20
    bg-white
    border-b
    flex
    items-center
    justify-between
    px-8">

    <button
        @click="ui.toggleSidebar()">

        <Menu />

    </button>

    <div class="flex items-center gap-6">

        <button
            class="
            relative
            p-2
            rounded-xl
            hover:bg-slate-100">

            <Bell />

            <span
                class="
                absolute
                top-0
                right-0
                h-2
                w-2
                rounded-full
                bg-red-500">
            </span>

        </button>

        <div
            class="relative">

            <button
                @click="dropdown=!dropdown"
                class="
                flex
                items-center
                gap-3">

                <div
                    class="
                    h-10
                    w-10
                    rounded-full
                    bg-[#614083]
                    text-white
                    flex
                    items-center
                    justify-center">

                    {{
                        auth.firstName
                            ?.charAt(0)
                    }}

                </div>

                <span>

                    {{
                        auth.firstName
                    }}

                </span>

                <ChevronDown />

            </button>

        </div>

    </div>

</header>

</template>