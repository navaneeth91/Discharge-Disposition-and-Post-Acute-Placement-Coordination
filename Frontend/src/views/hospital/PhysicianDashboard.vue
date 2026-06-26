<template>

<HospitalLayout>

    <PhysicianHeader />

    <template v-if="dashboard.currentSection === 'dashboard'">

        
        

    </template>

    <template v-else-if="dashboard.currentSection === 'patients'">


    </template>

    <template v-else-if="dashboard.currentSection === 'createDecision'">

        <DecisionModal />

    </template>

    <template v-else-if="dashboard.currentSection === 'pending'">


    </template>

</HospitalLayout>

</template>

<script setup>


import HospitalLayout from "@/layouts/HospitalLayout.vue";

import PhysicianHeader from "@/components/physician/PhysicianHeader.vue";



import { useDashboardStore } from "@/stores/dashboard";

const dashboard = useDashboardStore();

</script>