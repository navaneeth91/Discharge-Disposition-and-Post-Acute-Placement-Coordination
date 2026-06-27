<script setup>
import { onMounted } from 'vue'

import { useAuthStore } from '@/stores/auth'
import { useCareManagerStore } from '@/stores/careManager'

import AuthorizationTrackingTable from '@/components/careManager/AuthorizationTrackingTable.vue'

const auth = useAuthStore()

const careManager = useCareManagerStore()

onMounted(async () => {

    await careManager.loadAuthorizationTracking(
        auth.userId
    )

})

async function searchAuthorizations(search) {

    await careManager.searchAuthorizationTracking(

        auth.userId,

        search

    )

}

async function filterAuthorizations(status) {

    await careManager.filterAuthorizationTracking(

        auth.userId,

        status

    )

}

async function previousPage() {

    await careManager.previousAuthorizationPage(

        auth.userId

    )

}

async function nextPage() {

    await careManager.nextAuthorizationPage(

        auth.userId

    )

}

function viewAuthorization(authorization) {

    console.log(authorization)

    // Later you can open a modal here

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

            Authorization Tracking

        </h1>

        <p
            class="mt-2"
            style="
            color:var(--text-secondary);">

            Monitor insurance authorization requests for your patient referrals.

        </p>

    </div>

    <AuthorizationTrackingTable

        :authorizations="
            careManager.authorizationTracking"

        :loading="
            careManager.loading"

        :page="
            careManager.authorizationPage"

        :total-pages="
            careManager.authorizationTotalPages"

        @search="
            searchAuthorizations"

        @filter="
            filterAuthorizations"

        @previous="
            previousPage"

        @next="
            nextPage"

        @view="
            viewAuthorization" />

</div>

</template>