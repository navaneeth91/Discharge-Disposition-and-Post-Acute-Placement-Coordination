<script setup>
import {
    computed,
    onMounted,
    onUnmounted,
    ref,
    watch
}
from 'vue'
import { useDebounceFn } from '@vueuse/core'

import InsuranceLayout from '@/layouts/InsuranceLayout.vue'

import AuthorizationFilters
from '@/components/insurance/AuthorizationFilters.vue'

import AuthorizationTable
from '@/components/insurance/AuthorizationTable.vue'

import AuthorizationDetailsDrawer
from '@/components/insurance/AuthorizationDetailsDrawer.vue'

import {
    useInsuranceAuthorizationStore
}
from '@/stores/insuranceAuthorization'

const store =
    useInsuranceAuthorizationStore()
const refreshAuthorizations = async () => {

    await store.loadAuthorizations({
        search: search.value,
        status: status.value,
        page: page.value,
        pageSize: store.pagination.pageSize
    })

    await store.loadRecentInsuranceAuthorizations()

}
const search = ref('')
const status = ref('')
const page = ref(1)

const selectedAuthorization = ref(null)
const showDrawer = ref(false)

const title = computed(
    () => 'Authorization Requests'
)

const debouncedLoad =
    useDebounceFn(async () => {

        page.value = 1

        await store.loadAuthorizations({
            search: search.value,
            status: status.value,
            page: page.value,
            pageSize:
                store.pagination.pageSize
        })

    }, 350)

const loadPage =
    async (nextPage = 1) => {

        page.value = nextPage

        await store.loadAuthorizations({
            search: search.value,
            status: status.value,
            page: page.value,
            pageSize:
                store.pagination.pageSize
        })
    }

const resetFilters =
    async () => {

        search.value = ''
        status.value = ''
        page.value = 1

        await loadPage(1)
    }

const viewAuthorization =
    (authorization) => {

        selectedAuthorization.value =
            authorization

        showDrawer.value = true
    }

const approveAuthorization = async (authorization) => {
    try {

        await store.approveAuthorization(
            authorization.authorizationRequestId
        )

        await loadPage(page.value)

        showDrawer.value = false

    }
    catch (error) {

        console.error(
            'Approve failed:',
            error
        )
    }
}

const denyAuthorization = async (authorization) => {
    try {

        await store.denyAuthorization(
            authorization.authorizationRequestId,
            'Coverage Denied',
            'Authorization denied by insurance coordinator'
        )

        await loadPage(page.value)

        showDrawer.value = false

    }
    catch (error) {

        console.error(
            'Deny failed:',
            error
        )
    }
}

watch(
    search,
    () => debouncedLoad()
)

watch(
    status,
    () => loadPage(1)
)

onMounted(async () => {

    await store.loadAuthorizations()

    await store.loadRecentInsuranceAuthorizations()

    window.addEventListener(
        "refresh-authorizations",
        refreshAuthorizations
    )

})

onUnmounted(() => {

    window.removeEventListener(
        "refresh-authorizations",
        refreshAuthorizations
    )

})
</script>

<template>
    <InsuranceLayout>

        <div class="space-y-6 fade-up">

            <div>
                <h1
                    class="text-3xl font-bold text-[var(--text-primary)]">

                    {{ title }}

                </h1>

                <p
                    class="mt-2 text-[var(--text-secondary)]">

                    Search authorizations and filter them by status.

                </p>
            </div>

            <AuthorizationFilters
                v-model:search="search"
                v-model:status="status"
                :loading="store.loading"
                @reset="resetFilters" />

            <AuthorizationTable
                :items="store.authorizations"
                :loading="store.loading"
                :pagination="store.pagination"
                @page-change="loadPage"
                @select="viewAuthorization" />

            <AuthorizationDetailsDrawer
                :show="showDrawer"
                :authorization="selectedAuthorization"
                :loading="false"
                @close="showDrawer = false"
                @approve="approveAuthorization"
                @deny="denyAuthorization" />

        </div>

    </InsuranceLayout>
</template>