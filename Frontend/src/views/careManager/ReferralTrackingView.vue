<script setup>
import { ref, onMounted } from 'vue'

import { useAuthStore } from '@/stores/auth'
import { useCareManagerStore } from '@/stores/careManager'

import ReferralTable from '@/components/careManager/ReferralTable.vue'
import CreateAuthorizationModal from '@/components/careManager/CreateAuthorizationModal.vue'
const auth = useAuthStore()

const careManager = useCareManagerStore()

const showAuthorizationModal =
    ref(false)

const selectedReferral =
    ref(null)

onMounted(async () => {

    await careManager.loadReferralTracking(
        auth.userId
    )

})

function openAuthorizationModal(referral) {

    selectedReferral.value =
        referral

    showAuthorizationModal.value =
        true

}

function closeAuthorizationModal() {

    showAuthorizationModal.value =
        false

    selectedReferral.value =
        null

}

async function authorizationCreated() {

    closeAuthorizationModal()

    await careManager.loadReferralTracking(
        auth.userId
    )

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

            Monitor referrals and create insurance authorizations.

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

        @createAuthorization="
            openAuthorizationModal" />

    <CreateAuthorizationModal

        :show="
            showAuthorizationModal"

        :referral="
            selectedReferral"

        @close="
            closeAuthorizationModal"

        @created="
            authorizationCreated" />

</div>

</template>