<script setup>
import {
    ref,
    onMounted,
    onUnmounted
} from 'vue'

import { useAuthStore } from '@/stores/auth'
import { useCareManagerStore } from '@/stores/careManager'

import PatientTable from '@/components/careManager/PatientTable.vue'
import CreateReferralModal from '@/components/careManager/CreateReferralModal.vue'
import ViewReferralModal from '@/components/careManager/ViewReferralModal.vue'

const auth = useAuthStore()

const careManager = useCareManagerStore()

const showReferralModal = ref(false)

const showViewReferralModal = ref(false)

const selectedPatient = ref(null)
const refreshAssignments = async () => {

    await Promise.all([

        careManager.loadDashboard(
            auth.userId
        ),

        careManager.loadPatients(
            auth.userId
        )

    ])

}

onMounted(async () => {

    await careManager.loadDashboard(
        auth.userId
    )

    window.addEventListener(
        "refresh-assignments",
        refreshAssignments
    )

})

onUnmounted(() => {

    window.removeEventListener(
        "refresh-assignments",
        refreshAssignments
    )

})

function createReferral(patient) {

    selectedPatient.value = patient

    showReferralModal.value = true

}

function closeReferralModal() {

    showReferralModal.value = false

    selectedPatient.value = null

}

async function referralCreated() {

    closeReferralModal()

    await careManager.loadPatients(
        auth.userId
    )

}

function viewReferral(patient) {

    selectedPatient.value = patient

    showViewReferralModal.value = true

}

function closeViewReferralModal() {

    showViewReferralModal.value = false

    selectedPatient.value = null

}

async function searchPatients(search) {

    await careManager.searchPatients(
        auth.userId,
        search
    )

}

async function previousPage() {

    await careManager.previousPage(
        auth.userId
    )

}

async function nextPage() {

    await careManager.nextPage(
        auth.userId
    )

}
</script>

<template>

<div class="space-y-6">

    <div>

        <h1
            class="text-4xl font-bold"
            style="color: var(--text-primary);">

            My Patients

        </h1>

        <p
            class="mt-2"
            style="color: var(--text-secondary);">

            View and manage all patients assigned to you.

        </p>

    </div>

    <PatientTable

        :patients="careManager.assignedPatients"

        :loading="careManager.loading"

        :page="careManager.page"

        :total-pages="careManager.totalPages"

        @createReferral="createReferral"

        @viewReferral="viewReferral"

        @search="searchPatients"

        @previous="previousPage"

        @next="nextPage" />

    <CreateReferralModal

        :show="showReferralModal"

        :patient="selectedPatient"

        @close="closeReferralModal"

        @created="referralCreated" />

    <ViewReferralModal

        :show="showViewReferralModal"

        :patient="selectedPatient"

        @close="closeViewReferralModal" />

</div>

</template>