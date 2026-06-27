<script setup>
import StatCard
from '@/components/dashboard/StatCard.vue'

defineProps({

    dashboard:Object,

    loading:Boolean

})
</script>

<template>

<div
    class="
    
    grid
    grid-cols-1
    md:grid-cols-2
    xl:grid-cols-4
    gap-6">

    <StatCard

        title="Assigned Patients"

        :value="
        dashboard?.assignedPatients ?? 0"

        color="#003049"/>

    <StatCard

        title="Ready For Referral"

        :value="
        dashboard?.readyForReferral ?? 0"

        color="#669BBC"/>

    <StatCard

        title="Pending Referrals"

        :value="
        dashboard?.pendingReferrals ?? 0"

        color="#F77F00"/>

    <StatCard

        title="Active Delays"

        :value="
        dashboard?.activeDelays ?? 0"

        color="#D62828"/>

</div>

</template>