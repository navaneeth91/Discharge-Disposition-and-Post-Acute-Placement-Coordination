<script setup>
import {
    ref,
    computed,
    onMounted,
    watch
}
from 'vue'

import ViewReferralModal
from './ViewReferralModal.vue'

import * as providerService
from '@/services/providerService'

import {
    useDashboardStore
}
from '@/stores/dashboard'



import {
    useAuthStore
}
from '@/stores/auth'

const dashboard =
    useDashboardStore()

const auth =
    useAuthStore()

const referrals =
    ref([])

const loading =
    ref(true)

const loadingReferral =
    ref(false)

const selected =
    ref(null)

const showModal =
    ref(false)

const page =
    ref(1)

const pageSize =
    ref(5)

let searchTimeout = null

const props =
    defineProps({

        searchTerm: {

            type: String,

            default: ''
        },
        title: {

            type: String,

            default: 'All Referrals'

        },

        statusFilter: {

            type: String,

            default: 'All'
        },

        pendingOnly: {

            type: Boolean,

            default: false
        }

    })

async function loadReferrals() {

    try {

        loading.value = true
        console.log({
            userId: auth.userId,
            page: page.value,
            pageSize: pageSize.value,
            search: props.searchTerm,
            status: props.statusFilter
        })
        const status = props.pendingOnly
        ? 'Pending': props.statusFilter === 'All'
        ? null
        : props.statusFilter
        const response =
    await providerService
        .getProviderReferrals(

            auth.userId,

            page.value,

            pageSize.value,

            props.searchTerm,

            status

        )

        referrals.value =
            response.data.data

    }

    catch (error) {

        console.error(error)

    }

    finally {

        loading.value = false

    }

}

onMounted(loadReferrals)
watch(

    () => props.searchTerm,

    () => {

        page.value = 1

        clearTimeout(searchTimeout)

        searchTimeout = setTimeout(() => {

            loadReferrals()

        }, 500)

    }

)
watch(

    () => props.statusFilter,

    () => {

        page.value = 1

        loadReferrals()

    }

)
watch(

    page,

    () => {

        loadReferrals()

    }

)

async function viewReferral(referralId) {

    try {

        selected.value = null

        loadingReferral.value = true

        const response =
            await providerService
                .getReferralDetails(
                    referralId
                )
        
        selected.value =
            response.data.data

        showModal.value = true

    }

    catch (error) {

        console.error(error)

    }

    finally {

        loadingReferral.value = false

    }

}

async function acceptReferral(id) {

    try {

        await providerService
            .acceptReferral(id)

        await loadReferrals()
        await dashboard.loadProviderDashboard()

    }

    catch (error) {

        console.error(error)

    }

}

async function rejectReferral(id) {

    try {

        await providerService
            .rejectReferral(id)

        await loadReferrals()
        await dashboard.loadProviderDashboard()

    }

    catch (error) {

        console.error(error)

    }

}

function previousPage() {

    if (page.value > 1) {

        page.value--

    }

}

function nextPage() {

    if (referrals.value.length === pageSize.value) {

        page.value++

    }

}



function initials(name) {

    if (!name)

        return ''

    return name

        .charAt(0)

        .toUpperCase()

}

</script>

<template>

<div
    class="
    bg-white
    rounded-3xl
    p-8
    shadow-lg">

    <div
        class="
        flex
        justify-between
        items-center
        mb-8">

        <div>

            <h1
                class="
                text-3xl
                font-bold
                text-[#003049]">

                {{ title }}

            </h1>

            <p
                class="
                text-gray-500
                mt-1">

                {{ referrals.length }}
                referrals

            </p>

        </div>

    </div>

    <!-- Loading -->

    <div
        v-if="loading"
        class="
        flex
        justify-center
        py-16">

        <div
            class="
            text-gray-500">

            Loading referrals...

        </div>

    </div>

    <!-- Table -->

    <div
        v-else
        class="
        overflow-hidden
        rounded-2xl
        border
        border-gray-100">

        <table class="w-full">

            <thead
                class="
                bg-gray-50">

                <tr>

                    <th
                        class="
                        text-left
                        px-6
                        py-4
                        font-semibold">

                        Patient

                    </th>

                    <th
                        class="
                        text-left
                        px-6
                        py-4
                        font-semibold">

                        Facility

                    </th>

                    <th
                        class="
                        text-left
                        px-6
                        py-4
                        font-semibold">

                        Referral Date

                    </th>

                    <th
                        class="
                        text-left
                        px-6
                        py-4
                        font-semibold">

                        Status

                    </th>

                    <th
                        class="
                        text-right
                        px-6
                        py-4
                        font-semibold">

                        Actions

                    </th>

                </tr>

            </thead>

            <tbody>

                <tr
                    v-for="
                    referral in
                    referrals"

                    :key="
                    referral.referralId"

                    class="
                    border-t
                    hover:bg-blue-50
                    transition">

                    <!-- Patient -->

                    <td
                        class="
                        px-6
                        py-5">

                        <div
                            class="
                            flex
                            items-center
                            gap-4">

                            <div
                                class="
                                w-12
                                h-12
                                rounded-full
                                bg-[#003049]
                                text-white
                                flex
                                items-center
                                justify-center
                                font-semibold">

                                {{ initials(referral.patientName) }}

                            </div>

                            <div>

                                <div
                                    class="
                                    font-semibold
                                    text-[#003049]">

                                    {{ referral.patientName }}

                                </div>

                            </div>

                        </div>

                    </td>

                    <!-- Facility -->

                    <td
                        class="
                        px-6
                        py-5">

                        {{ referral.careManagerName }}

                    </td>

                    <!-- Date -->

                    <td
                        class="
                        px-6
                        py-5
                        text-gray-500">

                        {{ new Date(referral.createdDate).toLocaleDateString() }}

                    </td>

                    <!-- Status -->

                    <td
                        class="
                        px-6
                        py-5">

                        <span
                            class="
                            px-3
                            py-1
                            rounded-full
                            bg-blue-100
                            text-blue-700
                            text-sm">

                            {{ referral.status }}

                        </span>

                    </td>

                    <!-- Actions -->

                    <td
                        class="
                        px-6
                        py-5
                        text-right">

                        <div
                            class="
                            flex
                            justify-end
                            gap-2">

                            <button
                                @click="
                                viewReferral(
                                    referral.referralId
                                )"

                                class="
                                px-4
                                py-2
                                rounded-xl
                                bg-[#003049]
                                text-white
                                hover:bg-[#669BBC]
                                transition">

                                View

                            </button>

                            <button
                                v-if="referral.status === 'Pending'"

                                @click="
                                acceptReferral(
                                    referral.referralId
                                )"

                                class="
                                px-4
                                py-2
                                rounded-xl
                                bg-green-600
                                text-white
                                hover:bg-green-700
                                transition">

                                Approve

                            </button>

                            <button
                                v-if="referral.status === 'Pending'"

                                @click="
                                rejectReferral(
                                    referral.referralId
                                )"

                                class="
                                px-4
                                py-2
                                rounded-xl
                                bg-[#C1121F]
                                text-white
                                hover:bg-red-700
                                transition">

                                Deny

                            </button>

                        </div>

                    </td>

                </tr>

                <!-- Empty State -->

                <tr
                    v-if="
                    referrals.length === 0">

                    <td
                        colspan="5"
                        class="
                        text-center
                        py-10
                        text-gray-500">

                        No referrals found.

                    </td>

                </tr>

            </tbody>

        </table>

    </div>

    <!-- Loading Details -->

    <div
        v-if="loadingReferral"
        class="
        fixed
        inset-0
        bg-black/30
        flex
        items-center
        justify-center
        z-50">

        <div
            class="
            bg-white
            rounded-2xl
            px-8
            py-6
            shadow-xl">

            Loading referral details...

        </div>

    </div>

    <!-- Modal -->

    <ViewReferralModal

        :show="showModal"

        :referral="selected"

        @close="
        showModal = false;
        selected = null" />

    <div
    class="
    flex
    justify-between
    items-center
    mt-8">

    <div
        class="
        text-sm
        text-gray-500">

        Page {{ page }}

    </div>

    <div
        class="
        flex
        gap-2">

        <button
            @click="previousPage"
            :disabled="page === 1"

            class="
            px-4
            py-2
            rounded-xl
            border
            disabled:opacity-50">

            Previous

        </button>

        <button
            @click="nextPage"

            :disabled="
                referrals.length < pageSize
            "

            class="
            px-4
            py-2
            rounded-xl
            border
            disabled:opacity-50">

            Next

        </button>

    </div>

</div>

</div>

</template>