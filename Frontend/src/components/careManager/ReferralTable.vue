<script setup>
import { ref, watch } from 'vue'

import AppButton from '@/components/common/AppButton.vue'
import EmptyState from '@/components/common/EmptyState.vue'

const props = defineProps({

    referrals: {
        type: Array,
        default: () => []
    },

    loading: {
        type: Boolean,
        default: false
    },

    page: {
        type: Number,
        default: 1
    },

    totalPages: {
        type: Number,
        default: 1
    }

})

const emit = defineEmits([

    'search',

    'filter',

    'previous',

    'next',

    'createAuthorization'

])

const search =
    ref('')

const status =
    ref('')

let debounceTimer = null

watch(

    search,

    (value) => {

        clearTimeout(
            debounceTimer
        )

        debounceTimer =
            setTimeout(() => {

                emit(
                    'search',
                    value
                )

            }, 500)

    }

)

watch(

    status,

    (value) => {

        emit(
            'filter',
            value === ''
                ? null
                : value
        )

    }

)

function formatDate(date) {

    if (!date)
        return '-'

    return new Date(date)
        .toLocaleDateString()

}

function priorityClass(priority) {

    switch (priority) {

        case 'Critical':
            return 'bg-red-100 text-red-700'

        case 'High':
            return 'bg-orange-100 text-orange-700'

        case 'Normal':
            return 'bg-blue-100 text-blue-700'

        case 'Low':
            return 'bg-green-100 text-green-700'

        default:
            return 'bg-slate-100 text-slate-700'

    }

}

function statusClass(status) {

    switch (status) {

        case 'Pending':
            return 'bg-yellow-100 text-yellow-700'

        case 'Approved':
            return 'bg-green-100 text-green-700'

        case 'Denied':
            return 'bg-red-100 text-red-700'

        default:
            return 'bg-slate-100 text-slate-700'

    }

}
</script>
<template>

<div
    class="
    rounded-3xl
    p-6"
    style="
    background:white;
    box-shadow:var(--card-shadow);">

    <!-- Header -->

    <div
        class="
        flex
        flex-col
        lg:flex-row
        lg:items-center
        lg:justify-between
        gap-4
        mb-6">

        
        <div
            class="
            flex
            flex-col
            md:flex-row
            gap-3">

            <!-- Search -->

            <input

                v-model="search"

                type="text"

                placeholder="Search..."

                class="
                rounded-xl
                border
                px-4
                py-3
                outline-none"

                style="
                border-color:var(--border);" />

            <!-- Status Filter -->

            <select

                v-model="status"

                class="
                rounded-xl
                border
                px-4
                py-3
                outline-none"

                style="
                border-color:var(--border);">

                <option value="">

                    All Status

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

    <!-- Loading -->

    <div

        v-if="loading"

        class="
        py-12
        text-center">

        Loading referrals...

    </div>

    <!-- Empty -->

    <EmptyState

        v-else-if="referrals.length===0"

        title="No Referrals Found"

        message="No referrals match your search." />

    <!-- Table -->

    <div

        v-else

        class="overflow-x-auto">

        <table
            class="
            min-w-full
            divide-y
            divide-gray-200">

            <thead>

                <tr>

                    <th class="py-3 text-left">

                        Patient

                    </th>

                    <th class="py-3 text-left">

                        Provider

                    </th>

                    <th class="py-3 text-left">

                        Priority

                    </th>

                    <th class="py-3 text-left">

                        Status

                    </th>

                    <th class="py-3 text-left">

                        Created On

                    </th>

                    <th class="py-3 text-center">

                        Action

                    </th>

                </tr>

            </thead>

            <tbody>

                <tr

                    v-for="referral in referrals"

                    :key="referral.referralId"

                    class="border-b">

                    <td class="py-4">

                        {{ referral.patientName }}

                    </td>

                    <td>

                        {{ referral.providerName }}

                    </td>

                    <td>

                        <span

                            :class="[
                                'rounded-full px-3 py-1 text-sm font-medium',
                                priorityClass(referral.priority)
                            ]">

                            {{ referral.priority }}

                        </span>

                    </td>

                    <td>

                        <span

                            :class="[
                                'rounded-full px-3 py-1 text-sm font-medium',
                                statusClass(referral.status)
                            ]">

                            {{ referral.status }}

                        </span>

                    </td>

                    <td>

                        {{ formatDate(
                            referral.createdDate
                        ) }}

                    </td>
<td>

    <div
        class="
        flex
        justify-center">

        <!-- Pending -->

        <button

            v-if="referral.status === 'Pending'"

            disabled

            class="
            rounded-xl
            bg-slate-200
            text-slate-500
            px-4
            py-2
            font-medium
            cursor-not-allowed">

            Waiting for Provider

        </button>

        <!-- Approved & Authorization Not Created -->

        <AppButton

            v-else-if="
                referral.status === 'Approved' &&
                !referral.authorizationCreated
            "

            @click="
                emit(
                    'createAuthorization',
                    referral
                )">

            Create Authorization

        </AppButton>

        <!-- Approved & Authorization Already Created -->

        <button

            v-else-if="
                referral.status === 'Approved' &&
                referral.authorizationCreated
            "

            disabled

            class="
            rounded-xl
            bg-green-100
            text-green-700
            px-4
            py-2
            font-medium
            cursor-not-allowed">

            Authorization Created

        </button>

        <!-- Denied -->

        <button

            v-else

            disabled

            class="
            rounded-xl
            bg-red-100
            text-red-600
            px-4
            py-2
            font-medium
            cursor-not-allowed">

            Rejected

        </button>

    </div>

</td>
                </tr>

            </tbody>

        </table>

    </div>

    <!-- Pagination -->

    <div
        class="
        mt-6
        flex
        items-center
        justify-between
        border-t
        pt-4">

        <button

            @click="emit('previous')"

            :disabled="page===1"

            class="
            rounded-xl
            border
            px-4
            py-2
            disabled:opacity-50">

            ◀ Previous

        </button>

        <span
            class="font-medium">

            Page {{ page }}
            of
            {{ totalPages }}

        </span>

        <button

            @click="emit('next')"

            :disabled="page===totalPages"

            class="
            rounded-xl
            border
            px-4
            py-2
            disabled:opacity-50">

            Next ▶

        </button>

    </div>

</div>

</template>

<style scoped>

input,
select,
button{

    transition:.25s;

}

input:focus,
select:focus{

    border-color:#003049;

    box-shadow:
        0 0 0 4px
        rgba(102,155,188,.20);

}

button:hover:not(:disabled){

    border-color:#003049;

}

</style>