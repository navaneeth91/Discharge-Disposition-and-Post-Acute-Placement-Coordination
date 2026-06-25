<script setup>
import {
    LayoutDashboard,
    Users,
    ClipboardList,
    ShieldCheck,
    Building2,
    Activity,
    BarChart3,
    LogOut
}
from 'lucide-vue-next'

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
            font-bold
            border-b
            border-[#669BBC]/20">

            DDS

        </div>

        <nav class="flex-1 p-4 space-y-2">

            <!-- DASHBOARD -->

            <RouterLink
               :to="insurance
                     ? '/insurance/dashboard'
                    : '/administrator/dashboard'"
                     class="menu-item">

                <LayoutDashboard />

                <span
                    v-if="!ui.sidebarCollapsed">

                    Dashboard

                </span>

            </RouterLink>

            <!-- ADMINISTRATOR -->

            <template
                v-if="
                auth.role === 'Administrator'
                && !insurance">

                <RouterLink
                    to="/administrator/users"
                    class="menu-item">

                    <ShieldCheck />

                    <span
                        v-if="!ui.sidebarCollapsed">

                        Users

                    </span>

                </RouterLink>

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

                <RouterLink
                    to="/providers"
                    class="menu-item">

                    <Building2 />

                    <span
                        v-if="!ui.sidebarCollapsed">

                        Providers

                    </span>

                </RouterLink>

                <RouterLink
                    to="/los"
                    class="menu-item">

                    <Activity />

                    <span
                        v-if="!ui.sidebarCollapsed">

                        Length Of Stay

                    </span>

                </RouterLink>

            </template>

            <!-- CARE MANAGER -->

            <template
                v-if="
                auth.role === 'Care Manager'
                && !insurance">

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

            <!-- PHYSICIAN -->

            <template
                v-if="
                auth.role === 'Physician'
                && !insurance">

                <RouterLink
                    to="/patients"
                    class="menu-item">

                    <Users />

                    <span
                        v-if="!ui.sidebarCollapsed">

                        Patients

                    </span>

                </RouterLink>

            </template>

            <!-- POST ACUTE PROVIDER -->

            <template
                v-if="
                auth.role === 'Post-Acute Provider'
                && !insurance">

                <RouterLink
                    to="/providers"
                    class="menu-item">

                    <Building2 />

                    <span
                        v-if="!ui.sidebarCollapsed">

                        Providers

                    </span>

                </RouterLink>

            </template>

            <!-- AUTHORIZATION COORDINATOR -->

            <template
                v-if="
                auth.role ===
                'Authorization Coordinator'
                && !insurance">

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

            <!-- UNASSIGNED -->

            <template
                v-if="
                auth.role === 'Unassigned'
                && !insurance">

                <div
                    class="
                    p-4
                    text-center
                    text-sm
                    text-gray-300">

                    Awaiting role assignment

                </div>

            </template>
            <!-- INSURANCE -->

<template v-if="insurance">

    <RouterLink
        to="/insurance/authorizations"
        class="menu-item">

        <ClipboardList />

        <span
            v-if="!ui.sidebarCollapsed">

            Authorizations

        </span>

    </RouterLink>

    <RouterLink
        to="/insurance/members"
        class="menu-item">

        <Users />

        <span
            v-if="!ui.sidebarCollapsed">

            Members

        </span>

    </RouterLink>

    <RouterLink
        to="/insurance/providers"
        class="menu-item">

        <BarChart3 />

        <span
            v-if="!ui.sidebarCollapsed">

            Providers

        </span>

    </RouterLink>

    <RouterLink
        to="/insurance/plans"
        class="menu-item">

        <ShieldCheck />

        <span
            v-if="!ui.sidebarCollapsed">

            Plans

        </span>

    </RouterLink>

</template>

            <!-- LOGOUT -->

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
    font-weight: 500;
    padding: 14px;
    border-radius: 14px;
    transition: all 0.3s;
}

.menu-item:hover {
    background: rgba(102,155,188,.25);
    transform: translateX(4px);
}

.router-link-active {
    background: #669BBC;
    color: white;
}
</style>