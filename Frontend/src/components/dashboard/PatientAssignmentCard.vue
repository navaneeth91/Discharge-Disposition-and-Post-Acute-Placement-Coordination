<script setup>
import {
    ref,
    onMounted,
    watch
}
from 'vue'

import {
    useToast
}
from 'vue-toastification'

import {
    usePatientAssignmentStore
}
from '@/stores/patientAssignment'

const toast =
    useToast()

const store =
    usePatientAssignmentStore()

const selectedManagers =
    ref({})

const assigning =
    ref({})

const search =
    ref('')

const assignedBy =
    Number(
        JSON.parse(
            atob(
                sessionStorage
                    .getItem('token')
                    .split('.')[1]
            )
        ).sub
    )

onMounted(async () => {

    await store.loadData()

})

watch(

    search,

    async (value) => {

        await store.searchPatients(
            value
        )

    }

)

async function assign(patientId) {

    const managerId =
        selectedManagers.value[
            patientId
        ]

    if (!managerId) {

        toast.warning(
            'Please select a Care Manager.'
        )

        return

    }

    assigning.value[
        patientId
    ] = true

    try {

        await store.assignPatient(

            patientId,

            Number(managerId),

            assignedBy

        )

        delete selectedManagers.value[
            patientId
        ]

        toast.success(
            'Patient assigned successfully.'
        )

        if (
            store.unassignedPatients.length === 0 &&
            store.page < store.totalPages
        ) {

            await store.nextPage()

        }

    }

    catch (error) {

        console.error(error)

        toast.error(
            'Failed to assign patient.'
        )

    }

    finally {

        assigning.value[
            patientId
        ] = false

    }

}

async function nextPage() {

    await store.nextPage()

}

async function previousPage() {

    await store.previousPage()

}

async function goToPage(page) {

    await store.goToPage(page)

}
</script>
<template>

<div
    class="
    bg-white
    rounded-3xl
    p-6
    shadow-md">

    <!-- Header -->

    <div
        class="
        flex
        justify-between
        items-center
        mb-6">

        <div>

            <h2
                class="
                text-2xl
                font-bold
                text-[#003049]">

                Patient Assignment Center

            </h2>

            <p
                class="
                text-sm
                text-gray-500">

                Assign unassigned patients to Care Managers

            </p>

        </div>

        <div
            class="
            px-4
            py-2
            rounded-xl
            bg-[#EAF4F8]
            text-[#003049]
            font-semibold">

            {{ store.totalCount }}
            Pending Assignment(s)

        </div>

    </div>

    <!-- Search -->

    <div
        class="
        flex
        justify-between
        items-center
        mb-6">

        <input

            v-model="search"

            placeholder="Search patient..."

            class="
            w-96
            border
            border-[#CCDCE6]
            rounded-xl
            px-4
            py-3
            outline-none
            focus:ring-4
            focus:ring-[#669BBC]/20
            transition"/>

    </div>

    <!-- Table -->

    <div
        class="
        overflow-x-auto
        rounded-2xl
        border
        border-slate-200">

        <table
            class="
            w-full
            table-fixed">

            <thead
                class="
                bg-slate-50">

                <tr>

                    <th
                        class="
                        text-left
                        px-6
                        py-4
                        w-[28%]
                        font-semibold">

                        Patient

                    </th>

                    <th
                        class="
                        text-left
                        px-6
                        py-4
                        w-[18%]
                        font-semibold">

                        Department

                    </th>

                    <th
                        class="
                        text-left
                        px-6
                        py-4
                        w-[34%]
                        font-semibold">

                        Care Manager

                    </th>

                    <th
                        class="
                        text-center
                        px-6
                        py-4
                        w-[20%]
                        font-semibold">

                        Action

                    </th>

                </tr>

            </thead>

            <tbody>

                <!-- Loading -->

                <tr
                    v-if="store.loading">

                    <td
                        colspan="4"
                        class="
                        py-16
                        text-center
                        text-slate-500">

                        Loading unassigned patients...

                    </td>

                </tr>

                <!-- Empty -->

                <tr
                    v-else-if="
                    store.unassignedPatients.length === 0">

                    <td
                        colspan="4"
                        class="
                        py-16
                        text-center">

                        <div
                            class="
                            flex
                            flex-col
                            items-center
                            gap-2">

                            <div
                                class="text-5xl">

                                🎉

                            </div>

                            <div
                                class="
                                text-xl
                                font-semibold
                                text-[#003049]">

                                All patients have been assigned

                            </div>

                            <div
                                class="
                                text-gray-500">

                                There are currently no patients awaiting assignment.

                            </div>

                        </div>

                    </td>

                </tr>

                <!-- Patients -->

                <tr

                    v-for="
                    patient
                    in
                    store.unassignedPatients"

                    :key="
                    patient.patientId"

                    class="
                    border-t
                    hover:bg-[#F7FBFD]
                    transition">

                    <td
                        class="
                        px-6
                        py-5">

                        <div
                            class="
                            font-semibold
                            text-[#003049]">

                            {{ patient.firstName }}
                            {{ patient.lastName }}

                        </div>

                        <div
                            class="
                            text-xs
                            text-gray-500
                            mt-1">

                            {{ patient.mrn }}

                        </div>

                    </td>

                    <td
                        class="
                        px-6
                        py-5">

                        {{ patient.departmentName }}

                    </td>

                    <td
                        class="
                        px-6
                        py-5">

                        <select

                            v-model="
                            selectedManagers[
                            patient.patientId
                            ]"

                            class="
                            w-full
                            rounded-xl
                            border
                            border-slate-300
                            px-4
                            py-2.5
                            outline-none
                            bg-white
                            focus:ring-2
                            focus:ring-[#669BBC]
                            transition">

                            <option
                                value=""
                                disabled>

                                Select Care Manager

                            </option>

                            <option

                                v-for="
                                manager
                                in
                                store.careManagers"

                                :key="
                                manager.userId"

                                :value="
                                manager.userId">

                                {{ manager.firstName }}
                                {{ manager.lastName }}

                            </option>

                        </select>

                    </td>

                    <td
                        class="
                        px-6
                        py-5
                        text-center">

                        <button

                            @click="
                            assign(
                                patient.patientId
                            )"

                            :disabled="
                            !selectedManagers[
                            patient.patientId
                            ] ||
                            assigning[
                            patient.patientId
                            ]"

                            class="
                            w-44
                            rounded-xl
                            bg-[#003049]
                            py-2.5
                            text-white
                            font-medium
                            transition
                            hover:bg-[#669BBC]
                            disabled:opacity-50
                            disabled:cursor-not-allowed">

                            <span
                                v-if="
                                assigning[
                                patient.patientId
                                ]">

                                Assigning...

                            </span>

                            <span
                                v-else>

                                Assign

                            </span>

                        </button>

                    </td>

                </tr>

            </tbody>

        </table>

    </div>
        <!-- Pagination -->

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

</template>