<script setup>
import { onMounted } from 'vue'

import { useAuthStore } from '@/stores/auth'
import { useCareManagerStore } from '@/stores/careManager'

import WelcomeCard from '@/components/dashboard/WelcomeCard.vue'
import StatCard from '@/components/dashboard/StatCard.vue'
import PatientTable from '@/components/careManager/PatientTable.vue'

const auth = useAuthStore()
const careManager = useCareManagerStore()

onMounted(() => {
    careManager.loadDashboard(auth.userId)
})

function createReferral(patient) {
    console.log('Create Referral:', patient)
}

function viewReferral(patient) {
    console.log('View Referral:', patient)
}
</script>

<template>

<div class="space-y-8">

    <!-- Welcome -->

    <WelcomeCard />

    <!-- Dashboard Cards -->

    <div
        class="
        grid
        grid-cols-1
        md:grid-cols-2
        xl:grid-cols-4
        gap-6">

        <StatCard
            title="Assigned Patients"
            :value="careManager.dashboard?.assignedPatients ?? 0"
            color="#003049" />

        <StatCard
            title="Ready For Referral"
            :value="careManager.dashboard?.readyForReferral ?? 0"
            color="#669BBC" />

        <StatCard
            title="Pending Referrals"
            :value="careManager.dashboard?.pendingReferrals ?? 0"
            color="#F77F00" />

        <StatCard
            title="Active Delays"
            :value="careManager.dashboard?.activeDelays ?? 0"
            color="#D62828" />

    </div>

    <!-- Error -->

    <div
        v-if="careManager.error"
        class="
        rounded-2xl
        p-4
        bg-red-100
        text-red-700">

        {{ careManager.error }}

    </div>

    <!-- Patient Table -->

    
</div>

</template>
