<script setup>
import { onMounted }

from 'vue'

import InsuranceLayout
from '@/layouts/InsuranceLayout.vue'

import InsuranceChart
from '@/components/dashboard/InsuranceChart.vue'

import WelcomeCard
from '@/components/dashboard/WelcomeCard.vue'

import StatCard
from '@/components/dashboard/StatCard.vue'

import {
    useDashboardStore
}
from '@/stores/dashboard'

const dashboard =
    useDashboardStore()

onMounted(() => {

    dashboard
        .loadInsuranceDashboard()

})
</script>

<template>

<InsuranceLayout>

    <div class="space-y-8">

        <WelcomeCard />

        <div
            v-if="
            dashboard.insuranceStats"
            class="
                grid
                grid-cols-1
                sm:grid-cols-2
                lg:grid-cols-3
                gap-6">

            <StatCard
                title="Pending"
                :value="
                    dashboard.insuranceStats
                    .pending"
                color="#003049" />

            <StatCard
                title="Approved"
                :value="
                    dashboard.insuranceStats
                    .approved"
                color="#669BBC" />

            <StatCard
                title="Denied"
                :value="
                    dashboard.insuranceStats
                    .denied"
                color="#C1121F" />

        </div>
        <div class="mt-8">
            <InsuranceChart />
        </div>

    </div>

</InsuranceLayout>

</template>