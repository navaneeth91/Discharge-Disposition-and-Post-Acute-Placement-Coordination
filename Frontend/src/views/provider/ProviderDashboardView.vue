<script setup>
import {
    ref,
    onMounted
}
from 'vue'

import HospitalLayout
from '@/layouts/HospitalLayout.vue'

import * as providerService
from '@/services/providerService'

import ProviderWelcomeCard
from '@/components/provider/ProviderWelcomeCard.vue'

import StatCard
from '@/components/dashboard/StatCard.vue'

import PendingReferralsTable
from '@/components/provider/PendingReferralsTable.vue'

import {
    useDashboardStore
}
from '@/stores/dashboard'

const dashboard =
    useDashboardStore()

onMounted(() => {

    dashboard.loadProviderDashboard()

})





async function loadDashboard() {

    try {

        const response =
            await providerService
                .getDashboardSummary()

        providerStats.value =
            response.data.data

    }

    catch (error) {

        console.error(error)

    }

}
</script>

<template>

<HospitalLayout>

    <div class="space-y-8">

        <ProviderWelcomeCard />

        <div
            v-if="
            dashboard.providerStats"

            class="
            grid
            grid-cols-1
            sm:grid-cols-2
            lg:grid-cols-4
            gap-6">

            <StatCard
    title="Total Referrals"
    :value="dashboard.providerStats?.totalReferrals ?? 0"
    color="#003049" />

<StatCard
    title="Pending"
    :value="dashboard.providerStats?.pendingReferrals ?? 0"
    color="#669BBC" />

<StatCard
    title="Approved"
    :value="dashboard.providerStats?.approvedReferrals ?? 0"
    color="#16A34A" />

<StatCard
    title="Denied"
    :value="dashboard.providerStats?.rejectedReferrals ?? 0"
    color="#C1121F" />

        </div>

        <PendingReferralsTable />

    </div>

</HospitalLayout>

</template>