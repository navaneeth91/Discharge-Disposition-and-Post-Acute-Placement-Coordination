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
                    dashboard
                    .insuranceStats
                    .pending"
                color="#614083" />

            <StatCard
                title="Approved"
                :value="
                    dashboard
                    .insuranceStats
                    .approved"
                color="#79599B" />

            <StatCard
                title="Denied"
                :value="
                    dashboard
                    .insuranceStats
                    .denied"
                color="#9470B0" />

        </div>

    </div>

</InsuranceLayout>

</template>