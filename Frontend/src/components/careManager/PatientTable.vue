<script setup>
import { ref, watch } from 'vue'

import AppButton from '@/components/common/AppButton.vue'
import EmptyState from '@/components/common/EmptyState.vue'

const props = defineProps({

    patients: {
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

    'createReferral',

    'viewReferral',

    'search',

    'previous',

    'next'

])

const search =
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

function formatDate(date) {

    if (!date)
        return '-'

    return new Date(date)
        .toLocaleDateString()

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
        md:flex-row
        md:items-center
        md:justify-between
        gap-4
        mb-6">

        <div>

            <h2
                class="
                text-2xl
                font-bold"
                style="
                color:var(--text-primary);">

                Assigned Patients

            </h2>

            <p
                class="text-sm"
                style="
                color:var(--text-secondary);">

                Patients currently assigned to you

            </p>

        </div>

        <!-- Search -->

        <input

            v-model="search"

            type="text"

            placeholder="Search by Patient Name or MRN..."

            class="
            w-full
            md:w-80
            rounded-xl
            border
            px-4
            py-3
            outline-none"

            style="
            border-color:var(--border);" />

    </div>

    <!-- Loading -->

    <div

        v-if="loading"

        class="
        py-10
        text-center">

        Loading patients...

    </div>

    <!-- Empty -->

    <EmptyState

        v-else-if="patients.length===0"

        title="No Patients Assigned"

        message="There are currently no patients assigned to you." />

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
                        MRN
                    </th>

                    <th class="py-3 text-left">
                        Department
                    </th>

                    <th class="py-3 text-left">
                        Gender
                    </th>

                    <th class="py-3 text-left">
                        Expected Discharge
                    </th>

                    <th class="py-3 text-left">
                        Disposition
                    </th>

                    <th class="py-3 text-left">
                        Referral
                    </th>

                    <th class="py-3 text-center">
                        Action
                    </th>

                </tr>

            </thead>

            <tbody>

                <tr

                    v-for="patient in patients"

                    :key="patient.patientId"

                    class="border-b">

                    <td class="py-4">

                        {{ patient.patientName }}

                    </td>

                    <td>

                        {{ patient.mrn }}

                    </td>

                    <td>

                        {{ patient.departmentName }}

                    </td>

                    <td>

                        {{ patient.gender }}

                    </td>

                    <td>

                        {{ formatDate(
                            patient.expectedDischargeDate
                        ) }}

                    </td>

                    <td>

                        {{ patient.dispositionType ?? '-' }}

                    </td>

                    <td>

                        <span

                            v-if="patient.hasReferral"

                            class="
                            text-green-600
                            font-medium">

                            {{ patient.referralStatus }}

                        </span>

                        <span

                            v-else

                            class="
                            text-orange-500
                            font-medium">

                            Not Created

                        </span>

                    </td>

                    <td>

                        <div
                            class="
                            flex
                            justify-center">

                            <AppButton

                                :loading="false"

                                @click="
                                    patient.hasReferral
                                        ? emit('viewReferral', patient)
                                        : emit('createReferral', patient)
                                ">

                                {{ patient.hasReferral
                                    ? 'View Referral'
                                    : 'Create Referral' }}

                            </AppButton>

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

            :disabled="page === 1"

            class="
            px-4
            py-2
            rounded-xl
            border
            disabled:opacity-50">

            ◀ Previous

        </button>

        <span
            class="
            font-medium">

            Page {{ page }} of {{ totalPages }}

        </span>

        <button

            @click="emit('next')"

            :disabled="page === totalPages"

            class="
            px-4
            py-2
            rounded-xl
            border
            disabled:opacity-50">

            Next ▶

        </button>

    </div>

</div>

</template>

<style scoped>

input,
button{

    transition:.25s;

}

input:focus{

    border-color:#003049;

    box-shadow:
        0 0 0 4px
        rgba(102,155,188,.20);

}

button:hover:not(:disabled){

    border-color:#003049;

}

</style>