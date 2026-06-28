<script setup>
import {
    ref,
    onMounted,
    onUnmounted,
    watch
}
from 'vue'

import HospitalLayout
from '@/layouts/HospitalLayout.vue'

import ReferralDrawer
from '@/components/referrals/ReferralDrawer.vue'

import {
    useReferralStore
}
from '@/stores/referral'

const store = useReferralStore()
const refreshReferrals = async () => {

    await store.loadReferrals()

}
const search =
    ref('')

const selected =
    ref(null)

const showDrawer =
    ref(false)

onMounted(() => {

    store.loadReferrals()

    window.addEventListener(
        "refresh-referrals",
        refreshReferrals
    )

})

onUnmounted(() => {

    window.removeEventListener(
        "refresh-referrals",
        refreshReferrals
    )

})

let debounceTimer

watch(search, value => {

    clearTimeout(debounceTimer)

    debounceTimer = setTimeout(() => {

        store.searchReferrals(value)

    }, 300)

})

async function viewReferral(id) {

    await store.loadReferral(id)

    selected.value =
        store.selectedReferral

    showDrawer.value =
        true
}
function formatDate(date) {

    return new Intl.DateTimeFormat(

        'en-GB',

        {

            day: '2-digit',

            month: 'short',

            year: 'numeric'

        }

    ).format(new Date(date))

}
async function updateStatus(
    status
) {

    await store.updateStatus(
        selected.value.referralId,
        status
    )

    showDrawer.value =
        false
}
</script>

<template>

<HospitalLayout>

<div class="space-y-6">

    <div
        class="
        bg-white 
        rounded-3xl 
        p-8 
        shadow-lg">

        <!-- HEADER -->

        <div
            class="
            flex
            justify-between
            items-center
            mb-6">

            <div>

                <h1
                    class="
                    text-3xl
                    font-bold
                    text-[#2D1E3E]">

                    Referrals

                </h1>

                <p
                    class="
                    text-gray-500
                    mt-1">

                    {{ store.totalCount }}
                    referrals found

                </p>

            </div>

            <div
                class="
                flex
                gap-4">

                <input
                    v-model="search"

                    placeholder="Search referral..."

                    class="
                    w-72
                    border
                    rounded-xl
                    px-4
                    py-3
                    outline-none
                    border-gray-300
                    focus:ring-2
                    focus:ring-[#669BBC]">

                <select
                    v-model="store.status"
                    @change="
                    store.setStatus(
                        store.status
                    )"

                    class="
                    border
                    rounded-xl
                    px-4">

                    <option value="all">
                        All
                    </option>

                    <option value="Pending">
                        Pending
                    </option>

                    <option value="Approved">
                        Approved
                    </option>

                    <option value="Denied">
                        Denied
                    </option>

                </select>

            </div>

        </div>

        <!-- TABLE -->

        <div
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
                            Provider
                        </th>

                        <th
                            class="
                            text-left
                            px-6
                            py-4
                            font-semibold">
                            Care Manager
                        </th>

                        <th
                            class="
                            text-left
                            px-6
                            py-4
                            font-semibold">
                            Priority
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
                            text-left
                            px-6
                            py-4
                            font-semibold">
                            Date
                        </th>

                        <th
                            class="
                            text-left
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
                        store.referrals"

                        :key="
                        referral.referralId"

                        class="
                        border-t
                        hover:bg-[#F4F8FB]
                        transition">

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
                                        font-semibold
                                        text-[#003049]">
                                        {{
                                            referral.patientName
                                        }}
                                </div>
                            </div>
                        </td>

                        <td
                            class="
                            px-6
                            py-5">
                            
                            {{
                                referral.providerName
                            }}

                        </td>

                        <td
                            class="
                            px-6
                            py-5">

                            {{
                                referral.careManagerName
                            }}

                        </td>

                        <td>

                            <span
                                v-if="
                                referral.priority ===
                                'Critical'"

                                class="
                                px-4
                                py-1
                                rounded-full
                                bg-red-100
                                text-red-700
                                text-sm">

                                Critical

                            </span>

                            <span
                                v-else-if="
                                referral.priority ===
                                'High'"

                                class="
                                px-4
                                py-1
                                rounded-full
                                bg-orange-100
                                text-orange-700
                                text-sm">

                                High

                            </span>

                            <span
                                v-else-if="
                                referral.priority ===
                                'Low'"

                                class="
                                px-4
                                py-1
                                rounded-full
                                bg-blue-100
                                text-blue-700
                                text-sm">

                                Low

                            </span>

                            <span
                                v-else

                                class="
                                px-4
                                py-1
                                rounded-full
                                bg-gray-100
                                text-gray-700
                                text-sm">

                                Normal

                            </span>

                        </td>

                        <td>

                            <span
                                v-if="
                                referral.status ===
                                'Approved'"

                                class="
                                px-4
                                py-1
                                rounded-full
                                bg-green-100
                                text-green-700
                                text-sm">

                                Approved

                            </span>

                            <span
                                v-else-if="
                                referral.status ===
                                'Denied'"

                                class="
                                px-4
                                py-1
                                rounded-full
                                bg-red-100
                                text-red-700
                                text-sm">

                                Denied

                            </span>

                            <span
                                v-else

                                class="
                                px-4
                                py-1
                                rounded-full
                                bg-yellow-100
                                text-yellow-700
                                text-sm">

                                Pending

                            </span>

                        </td>

                        <td
                            class="
                            px-6
                            py-5">

                            {{ formatDate(referral.createdDate) }}

                        </td>

                        <td
                            class="
                            text-center">

                            <button
                                @click="
                                viewReferral(
                                    referral.referralId
                                )"

                                class="
                                px-5
                                py-2
                                rounded-xl
                                bg-[#003049]
                                text-white
                                hover:bg-[#00243A]
                                transition">

                                View

                            </button>

                        </td>

                    </tr>

                </tbody>

            </table>

        </div>

        <!-- PAGINATION -->

        <div
            class="
            flex
            justify-between
            items-center
            mt-8">

            <button
                @click="
                store.previousPage()
                "

                :disabled="
                store.page === 1"

                class="
                px-4
                py-2
                rounded-xl
                bg-slate-100">

                Previous

            </button>

            <p>

                Page
                {{ store.page }}
                of
                {{ store.totalPages }}

            </p>

            <button
                @click="
                store.nextPage()
                "

                :disabled="
                store.page ===
                store.totalPages"

                class="
                px-4
                py-2
                rounded-xl
                bg-[#003049]
                text-white">

                Next

            </button>

        </div>

    </div>

    <ReferralDrawer
        :show="showDrawer"
        :referral="selected"
        @close="
        showDrawer=false"
        @status="
        updateStatus" />

</div>

</HospitalLayout>

</template>