<script setup>
import { onMounted }

from 'vue'

import InsuranceLayout
from '@/layouts/InsuranceLayout.vue'

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
            md:grid-cols-4
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

    </div>

</InsuranceLayout>

</template>