<script setup>
import { ref, computed, onMounted }
from 'vue'

import HospitalLayout from '@/layouts/HospitalLayout.vue'
import PatientDrawer from '@/components/patients/PatientDrawer.vue'
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
onMounted(async () => {

    await store.loadPatients()

})

const filtered =
    computed(() => {

        return store.patients.filter(
            p =>

                p.firstName
                    ?.toLowerCase()
                    .includes(
                        search.value
                        .toLowerCase()
                    )

                ||

                p.lastName
                    ?.toLowerCase()
                    .includes(
                        search.value
                        .toLowerCase()
                    )
        )
    })
</script>

<template>

<HospitalLayout>

<div class="space-y-6 fade-up">

    <div
        class="
        bg-white
        rounded-3xl
        p-6
        shadow-md">

        <div
            class="
            flex
            justify-between
            items-center
            mb-6">

            <h1
                class="
                text-3xl
                font-bold
                text-[#2D1E3E]">

                Patients

            </h1>

            <input
                v-model="search"
                placeholder="Search patient..."
                class="
                w-80
                border
                border-[#DDD2E8]
                rounded-xl
                px-4
                py-3
                outline-none
                focus:ring-4
                focus:ring-purple-100">
        </div>

        <div class="overflow-x-auto">

            <table
                class="
                w-full">

                <thead>

                    <tr
                        class="
                        border-b">

                        <th class="text-left py-4">
                            Name
                        </th>

                        <th>
                            Admission
                        </th>

                        <th>
                            Status
                        </th>

                        <th>
                            Actions
                        </th>

                    </tr>

                </thead>

                <tbody>

                    <tr
                        v-for="patient in filtered"
                        :key="
                            patient.patientId"

                        class="
                        border-b
                        hover:bg-purple-50
                        transition">

                        <td
                            class="py-5">

                            {{ patient.firstName }}
                            {{ patient.lastName }}

                        </td>

                        <td>

                            {{
                                patient.admissionDate
                            }}

                        </td>

                        <td>

                            <span
                                v-if="
                                patient.isActive"

                                class="
                                px-3
                                py-1
                                rounded-full
                                bg-green-100
                                text-green-600">

                                Active

                            </span>

                            <span
                                v-else

                                class="
                                px-3
                                py-1
                                rounded-full
                                bg-red-100
                                text-red-600">

                                Discharged

                            </span>

                        </td>

                        <td>

                            <button
                                @click="
                                viewPatient(
                                    patient.patientId
                                )"

                                class="
                                text-[#614083]
                                font-medium">

                                View

                            </button>

                        </td>

                    </tr>

                </tbody>

            </table>

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