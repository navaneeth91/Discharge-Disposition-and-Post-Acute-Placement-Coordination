<script setup>
import {
    LayoutDashboard,
    Users,
    ClipboardList,
    Clock3,
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

const menuItems = [

    {
        label: 'Dashboard',
        icon: LayoutDashboard,
        route: '/care-manager/dashboard'
    },

    {
        label: 'My Patients',
        icon: Users,
        route: '/care-manager/patients'
    },

    {
        label: 'Referrals Tracking',
        icon: ClipboardList,
        route: '/care-manager/referrals/'
    }
]

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
    fixed
    left-0
    top-0
    h-screen
    bg-[#003049]
    text-white
    transition-all
    duration-300
    flex
    flex-col
    z-50">

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

    <nav
        class="
        flex-1
        p-4
        space-y-2">

        <RouterLink
            v-for="item in menuItems"
            :key="item.route"
            :to="item.route"
            class="menu-item">

            <component :is="item.icon" />

            <span
                v-if="!ui.sidebarCollapsed">

                {{ item.label }}

            </span>

        </RouterLink>

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

.menu-item{

    display:flex;

    align-items:center;

    gap:14px;

    font-weight:500;

    padding:14px;

    border-radius:14px;

    transition:all .3s;

}

.menu-item:hover{

    background:rgba(102,155,188,.25);

}

.router-link-active{

    background:#669BBC;

    color:white;

}

</style>