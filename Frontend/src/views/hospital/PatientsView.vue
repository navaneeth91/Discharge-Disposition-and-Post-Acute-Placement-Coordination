<script setup>
import {
    ref,
    computed,
    onMounted,
    watch
}
from 'vue'

import HospitalLayout
from '@/layouts/HospitalLayout.vue'

import PatientDrawer
from '@/components/patients/PatientDrawer.vue'

import {
    usePatientStore
}
from '@/stores/patient'

const store =
    usePatientStore()

const selected =
    ref(null)

const showDrawer =
    ref(false)

const search =
    ref('')

async function viewPatient(id) {

    await store.loadPatient(id)

    selected.value =
        store.selectedPatient

    showDrawer.value =
        true
}

async function discharge(date) {

    await store.discharge(
        selected.value.patientId,
        date
    )

    showDrawer.value =
        false
}

watch(search, async value => {

    await store.searchPatients(
        value
    )
})

onMounted(async () => {

    await store.loadPatients()
})

const patients =
    computed(() => {

        return store.patients
    })

function initials(patient) {

    return (
        patient.firstName[0] +
        patient.lastName[0]
    ).toUpperCase()
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

                    Patients

                </h1>

                <p class="text-slate-500">

                    {{ store.totalCount }}
                    patients

                </p>

            </div>

            <input
                v-model="search"
                placeholder="Search patients..."

                class="
                border
                border-slate-200
                rounded-xl
                p-3
                w-80
                outline-none
                focus:ring-4
                focus:ring-[#669BBC]/30">
        </div>

        <div class="flex gap-3 mb-6">

            <button
                @click="
                store.setStatus(
                    'all'
                )"

                :class="
                store.status === 'all'
                ? 'bg-[#003049] text-white'
                : 'bg-slate-100'
                "

                class="
                px-4
                py-2
                rounded-xl">

                All

            </button>

            <button
                @click="
                store.setStatus(
                    'active'
                )"

                :class="
                store.status === 'active'
                ? 'bg-green-600 text-white'
                : 'bg-slate-100'
                "

                class="
                px-4
                py-2
                rounded-xl">

                Active

            </button>

            <button
                @click="
                store.setStatus(
                    'discharged'
                )"

                :class="
                store.status === 'discharged'
                ? 'bg-red-600 text-white'
                : 'bg-slate-100'
                "

                class="
                px-4
                py-2
                rounded-xl">

                Discharged

            </button>

        </div>

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
                            Department
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
                            Admission
                        </th>

                        <th></th>

                    </tr>

                </thead>

                <tbody>

                    <tr
                        v-for="patient in patients"
                        :key="patient.patientId"

                        class="
                        border-t
                        hover:bg-blue-50
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
                                    w-12
                                    h-12
                                    rounded-full
                                    bg-[#003049]
                                    text-white
                                    flex
                                    items-center
                                    justify-center
                                    font-semibold">

                                    {{
                                        patient.firstName[0]
                                    }}
                                    {{
                                        patient.lastName[0]
                                    }}

                                </div>

                                <div
                                        class="
                                        font-semibold
                                        text-[#003049]">
                                        {{
                                            patient.firstName
                                        }}
                                        {{
                                            patient.lastName
                                        }}

                                    

                                    <div
                                        class="
                                        text-sm
                                        text-gray-500">

                                        {{
                                            patient.mrn
                                        }}

                                    </div>

                                </div>

                            </div>

                        </td>

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

                                {{
                                    patient.departmentName
                                }}
                                </span>

                        </td>

                        <td
                            class="
                            px-6
                            py-5">

                            <span
                                v-if="patient.isActive"

                                class="
                                px-3
                                py-1
                                rounded-full
                                bg-green-100
                                text-green-700
                                text-sm">

                                Active

                            </span>

                            <span
                                v-else

                                class="
                                px-4
                                py-1
                                rounded-full
                                bg-red-100
                                text-red-700
                                text-sm">

                                Discharged

                            </span>

                        </td>

                        <td>

                            {{
                                new Date(
                                    patient.admissionDate
                                ).toLocaleDateString()
                            }}

                        </td>

                        <td class="text-center">

                            <button
                                @click="
                                viewPatient(
                                    patient.patientId
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

</div>

<PatientDrawer
    :patient="selected"
    :show="showDrawer"
    @close="showDrawer=false"
    @discharge="discharge" />

</HospitalLayout>

</template>