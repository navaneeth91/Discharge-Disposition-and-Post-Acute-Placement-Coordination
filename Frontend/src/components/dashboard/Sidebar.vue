<script setup>
import {
    LayoutDashboard,
    Users,
    ClipboardList,
    ShieldCheck,
    BarChart3,
    Settings,
    LogOut
} from 'lucide-vue-next'

import { useUiStore }
from '@/stores/ui'

import { useRouter }
from 'vue-router'

import { useAuthStore }
from '@/stores/auth'

const ui = useUiStore()

const auth = useAuthStore()

const router = useRouter()

const props = defineProps({
    insurance: Boolean
})

function logout() {

    auth.logout()

    router.push('/login')
}
</script>

<template>

    <aside
        :class="[
            ui.sidebarCollapsed
                ? 'w-24'
                : 'w-72'
        ]"

        class="
        h-screen
        bg-[#4F336D]
        text-white
        transition-all
        duration-300
        flex
        flex-col">

        <div
            class="
            h-20
            flex
            items-center
            justify-center
            text-2xl
            font-bold">

            DDS

        </div>

        <nav class="flex-1 p-4 space-y-2">

            <RouterLink
                to="/administrator/dashboard"
                class="menu-item">

                <LayoutDashboard />

                <span
                    v-if="!ui.sidebarCollapsed">

                    Dashboard

                </span>

            </RouterLink>

            <template v-if="!insurance">

                <RouterLink
                    to="/patients"
                    class="menu-item">

                    <Users />

                    <span
                        v-if="!ui.sidebarCollapsed">

                        Patients

                    </span>

                </RouterLink>

                <RouterLink
                    to="/referrals"
                    class="menu-item">

                    <ClipboardList />

                    <span
                        v-if="!ui.sidebarCollapsed">

                        Referrals

                    </span>

                </RouterLink>

            </template>

            <button
                @click="logout"
                class="menu-item w-full">

                <LogOut />

                <span
                    v-if="!ui.sidebarCollapsed">

                    Logout

                </span>

            </button>

        </nav>

    </aside>

</template>

<style scoped>
.menu-item {
    display: flex;
    align-items: center;
    gap: 14px;
    padding: 14px;
    border-radius: 14px;
    transition: all 0.3s;
}

.menu-item:hover {
    background: #614083;
}

.router-link-active {
    background: #79599B;
}
</style>