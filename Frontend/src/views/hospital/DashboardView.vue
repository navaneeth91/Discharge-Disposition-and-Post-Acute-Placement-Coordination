<script setup>
import { onMounted }

from 'vue'

import HospitalLayout
from '@/layouts/HospitalLayout.vue'

import WelcomeCard
from '@/components/dashboard/WelcomeCard.vue'

import StatCard
from '@/components/dashboard/StatCard.vue'
import PatientChart
from '@/components/dashboard/PatientChart.vue'

import AuthorizationChart
from '@/components/dashboard/AuthorizationChart.vue'
import {
    useDashboardStore
}
from '@/stores/dashboard'

const dashboard =
    useDashboardStore()

onMounted(() => {

    dashboard
        .loadHospitalDashboard()

})
</script>

<template>

<HospitalLayout>

    <div class="space-y-8">

        <WelcomeCard />

        <div
            v-if="dashboard.hospitalStats"
            class="
            grid
            grid-cols-1
            md:grid-cols-2
            xl:grid-cols-4
            gap-6">

            <StatCard
                title="Patients"
                :value="
                    dashboard.hospitalStats
                    .totalPatients"
                color="#003049" />

            <StatCard
                title="Pending Referrals"
                :value="
                    dashboard.hospitalStats
                    .pendingReferrals"
                color="#669BBC" />

            <StatCard
                title="Pending Authorizations"
                :value="
                    dashboard.hospitalStats
                    .pendingAuthorizations"
                color="#C1121F" />

            <StatCard
                title="Active Delays"
                :value="
                    dashboard.hospitalStats
                    .activeDelays"
                color="#780000" />

        </div>
        <div
            class="
            grid
            lg:grid-cols-2
            gap-6">

            <PatientChart />

            <AuthorizationChart />

        </div>

    </div>

</HospitalLayout>

</template>
