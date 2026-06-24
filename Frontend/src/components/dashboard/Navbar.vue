<script setup>
import {
    Bell,
    Menu,
    ChevronDown,
    User,
    Settings,
    LogOut
}
from 'lucide-vue-next'

import { ref } from 'vue'
import { useRouter } from 'vue-router'

import { useUiStore }
from '@/stores/ui'

import { useAuthStore }
from '@/stores/auth'

const ui = useUiStore()
const auth = useAuthStore()
const router = useRouter()

const profileDropdown = ref(false)
const notificationDropdown = ref(false)

function logout() {

    sessionStorage.clear()

    auth.logout()

    router.push('/login')
}
</script>

<template>

<header
    class="
    h-20
    bg-white
    border-b
    border-slate-200
    flex
    items-center
    justify-between
    px-8
    shadow-sm">

    <!-- Sidebar Toggle -->

    <button
        @click="ui.toggleSidebar()"
        class="
        p-2
        rounded-xl
        hover:bg-[#FDF0D5]
        transition">

        <Menu
            class="text-[#003049]" />

    </button>

    <div class="flex items-center gap-6">

        <!-- Notifications -->

        <div class="relative">

            <button
                @click="
                notificationDropdown =
                !notificationDropdown"

                class="
                relative
                p-3
                rounded-xl
                hover:bg-[#FDF0D5]
                transition">

                <Bell
                    class="text-[#003049]" />

                <span
                    class="
                    absolute
                    top-2
                    right-2
                    h-2
                    w-2
                    rounded-full
                    bg-[#C1121F]">
                </span>

            </button>

            <div
                v-if="notificationDropdown"
                class="
                absolute
                right-0
                mt-3
                w-80
                bg-white
                rounded-2xl
                shadow-xl
                border
                z-50">

                <div
                    class="
                    px-5
                    py-4
                    border-b
                    font-semibold
                    text-[#003049]">

                    Notifications

                </div>

                <div
                    class="
                    p-5
                    text-slate-500">

                    No notifications.

                </div>

            </div>

        </div>

        <!-- Profile -->

        <div class="relative">

            <button
                @click="
                profileDropdown =
                !profileDropdown"

                class="
                flex
                items-center
                gap-3
                hover:bg-[#FDF0D5]
                rounded-xl
                px-3
                py-2
                transition">

                <div
                    class="
                    h-10
                    w-10
                    rounded-full
                    text-white
                    flex
                    items-center
                    justify-center
                    font-semibold"

                    style="
                    background:
                    var(--primary);">

                    {{
                        auth.firstName
                            ?.charAt(0)
                    }}

                </div>

                <div class="text-left">

                    <div
                        class="
                        font-semibold
                        text-[#003049]">

                        {{ auth.firstName }}

                    </div>

                    <div
                        class="
                        text-sm
                        text-slate-500">

                        {{ auth.role }}

                    </div>

                </div>

                <ChevronDown
                    class="
                    text-slate-500" />

            </button>

            <!-- Dropdown -->

            <div
                v-if="profileDropdown"
                class="
                absolute
                right-0
                mt-3
                w-64
                bg-white
                rounded-2xl
                shadow-xl
                border
                overflow-hidden
                z-50">

                <button
                    class="
                    w-full
                    px-5
                    py-4
                    flex
                    items-center
                    gap-3
                    hover:bg-[#FDF0D5]
                    transition">

                    <User size="18" />

                    Profile

                </button>

                <button
                    class="
                    w-full
                    px-5
                    py-4
                    flex
                    items-center
                    gap-3
                    hover:bg-[#FDF0D5]
                    transition">

                    <Settings size="18" />

                    Settings

                </button>

                <button
                    @click="logout"

                    class="
                    w-full
                    px-5
                    py-4
                    flex
                    items-center
                    gap-3
                    text-[#C1121F]
                    hover:bg-red-50
                    transition">

                    <LogOut size="18" />

                    Logout

                </button>

            </div>

        </div>

    </div>

</header>

</template>