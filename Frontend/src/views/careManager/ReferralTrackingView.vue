<script setup>
import { ref, onMounted } from 'vue'

import { useAuthStore } from '@/stores/auth'
import { useCareManagerStore } from '@/stores/careManager'

import ReferralTable from '@/components/careManager/ReferralTable.vue'
import ViewReferralModal from '@/components/careManager/ViewReferralModal.vue'

const auth = useAuthStore()

const careManager = useCareManagerStore()

const showViewModal =
    ref(false)

const selectedReferral =
    ref(null)

onMounted(async () => {

    await careManager.loadReferralTracking(
        auth.userId
    )

})

function viewReferral(referral) {

    selectedReferral.value = {
        patientId: referral.patientId
    }

    showViewModal.value = true

}

function closeModal() {

    showViewModal.value = false

    selectedReferral.value = null

}

async function searchReferrals(search) {

    await careManager.searchReferrals(
        auth.userId,
        search
    )

}

async function filterReferrals(status) {

    await careManager.filterReferrals(
        auth.userId,
        status
    )

}

async function previousPage() {

    await careManager.previousReferralPage(
        auth.userId
    )

}

async function nextPage() {

    await careManager.nextReferralPage(
        auth.userId
    )

}
</script>

<template>

<div class="space-y-6">

    <div>

        <h1
            class="
            text-4xl
            font-bold"
            style="
            color:var(--text-primary);">

            Referral Tracking

        </h1>

        <p
            class="mt-2"
            style="
            color:var(--text-secondary);">

            Monitor all referrals created for your assigned patients.

        </p>

    </div>

    <ReferralTable

        :referrals="
            careManager.referrals"

        :loading="
            careManager.loading"

        :page="
            careManager.referralPage"

        :total-pages="
            careManager.referralTotalPages"

        @search="
            searchReferrals"

        @filter="
            filterReferrals"

        @previous="
            previousPage"

        @next="
            nextPage"

        @view="
            viewReferral" />

    <ViewReferralModal

        :show="
            showViewModal"

        :patient="
            selectedReferral"

        @close="
            closeModal" />

</div>

</template>