<script setup>
import {
    onMounted,
    onUnmounted
} from 'vue'

import InsuranceLayout from '@/layouts/InsuranceLayout.vue'
import InsuranceChart from '@/components/dashboard/InsuranceChart.vue'
import WelcomeCard from '@/components/dashboard/WelcomeCard.vue'
import StatCard from '@/components/dashboard/StatCard.vue'
import RecentAuthorizationCard from '@/components/insurance/RecentAuthorizationCard.vue'
import { useDashboardStore } from '@/stores/dashboard'

const dashboard = useDashboardStore()
const refreshDashboard = async () => {

    await dashboard.loadInsuranceDashboard()

    await dashboard.loadRecentInsuranceAuthorizations()

}
onMounted(async () => {

    await dashboard.loadInsuranceDashboard()

    await dashboard.loadRecentInsuranceAuthorizations()

    window.addEventListener(
        "refresh-dashboard",
        refreshDashboard
    )

})

onUnmounted(() => {

    window.removeEventListener(
        "refresh-dashboard",
        refreshDashboard
    )

})
</script>

<template>
    <InsuranceLayout>
        <div class="space-y-8">
            <WelcomeCard />

            <div
                v-if="dashboard.insuranceStats"
                class="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
                <StatCard title="Pending" :value="dashboard.insuranceStats.pending" color="#003049" />
                <StatCard title="Approved" :value="dashboard.insuranceStats.approved" color="#669BBC" />
                <StatCard title="Denied" :value="dashboard.insuranceStats.denied" color="#C1121F" />
            </div>

            <div class="grid gap-6 xl:grid-cols-[1.4fr_1fr]">
                <InsuranceChart />
                <RecentAuthorizationCard :items="dashboard.recentInsuranceAuthorizations" :loading="dashboard.recentLoading" />
            </div>
        </div>
    </InsuranceLayout>
</template>