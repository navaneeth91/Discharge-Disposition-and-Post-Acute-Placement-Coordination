<script setup>
import AllReferralsTable
from './AllReferralsTable.vue'

defineProps({

    searchTerm: String
})
</script>

<template>

<AllReferralsTable
    :searchTerm="searchTerm"
    statusFilter="All"
    :pendingOnly="true"
    title="Pending Referrals" />

</template>