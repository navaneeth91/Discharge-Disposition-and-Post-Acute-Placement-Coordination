<script setup>
import { ref, watch } from 'vue'

import AppButton from '@/components/common/AppButton.vue'
import EmptyState from '@/components/common/EmptyState.vue'

const props = defineProps({

    authorizations: {
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

    'view'

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

    <!-- Search & Filter -->

    <div
        class="
        flex
        flex-col
        lg:flex-row
        lg:justify-end
        gap-3
        mb-6">

        <input

            v-model="search"

            type="text"

            placeholder="Search Patient / Payer / Authorization ID"

            class="
            rounded-xl
            border
            px-4
            py-3
            outline-none"

            style="
            border-color:var(--border);" />

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

    <!-- Loading -->

    <div

        v-if="loading"

        class="
        py-12
        text-center">

        Loading authorization tracking...

    </div>

    <!-- Empty -->

    <EmptyState

        v-else-if="authorizations.length === 0"

        title="No Authorization Records"

        message="No authorization tracking records found." />

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

                        Payer

                    </th>

                    <th class="py-3 text-left">

                        Authorization ID

                    </th>

                    <th class="py-3 text-left">

                        Status

                    </th>

                    <th class="py-3 text-left">

                        Requested

                    </th>

                    <th class="py-3 text-left">

                        Response

                    </th>

                    

                </tr>

            </thead>

            <tbody>

                <tr

                    v-for="authorization in authorizations"

                    :key="authorization.authorizationId"

                    class="border-b">

                    <td class="py-4">

                        {{ authorization.patientName }}

                    </td>

                    <td>

                        {{ authorization.payerName }}

                    </td>

                    <td>

                        {{ authorization.externalAuthorizationId }}

                    </td>

                    <td>

                        <span

                            :class="[
                                'rounded-full px-3 py-1 text-sm font-medium',
                                statusClass(
                                    authorization.status
                                )
                            ]">

                            {{ authorization.status }}

                        </span>

                    </td>

                    <td>

                        {{ formatDate(
                            authorization.requestedDate
                        ) }}

                    </td>

                    <td>

                        {{ formatDate(
                            authorization.responseDate
                        ) }}

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

            :disabled="page === 1"

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

            :disabled="page === totalPages"

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