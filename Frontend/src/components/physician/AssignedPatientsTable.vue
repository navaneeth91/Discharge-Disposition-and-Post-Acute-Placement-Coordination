<template>
    <HospitalLayout>
        <div class="bg-white rounded-2xl shadow-lg p-6">

            <!-- Header -->
            <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4 mb-6">

            <div>
                <h2
                class="text-xl font-bold"
                style="color: var(--primary);"
                >
                Assigned Patients
                </h2>

                <p
                class="text-sm mt-1"
                style="color: var(--text-secondary);"
                >
                Patients assigned for disposition review
                </p>
            </div>

            <input
                v-model="search"
                type="text"
                placeholder="Search patient..."
                class="w-full md:w-72 rounded-xl border px-4 py-2 outline-none focus:ring-2"
                style="
                border-color: var(--border);
                /* focus:ring-color: var(--secondary); */
                "
            />

            </div>

            <!-- Table -->

            <div class="overflow-x-auto">

            <table class="min-w-full">

                <thead class="bg-gray-50">

                    <tr
                        class="border-b"
                        style="border-color: var(--border);"
                    >

                        <th class="text-left px-5 py-4 font-semibold text-[var(--primary)]">
                            Patient
                        </th>

                        <th class="text-left px-5 py-4 font-semibold text-[var(--primary)]">
                            Disposition
                        </th>

                        <th class="text-left px-5 py-4 font-semibold text-[var(--primary)]">
                            Status
                        </th>

                        <th class="text-center px-5 py-4 font-semibold text-[var(--primary)]">
                            Actions
                        </th>

                    </tr>

                </thead>

                <tbody>

                    <tr
                        v-for="patient in paginatedPatients"
                        :key="patient.patientId"
                        class="border-b hover:bg-gray-50 transition"
                        style="border-color: var(--border);"
                    >

                        <!-- Patient -->

                        <td class="py-4">

                            <div class="flex items-center gap-4">

                                <div
                                    class="w-12 h-12 rounded-full bg-[var(--primary)] text-white flex items-center justify-center font-bold text-sm shrink-0"
                                >

                                    {{ getInitials(patient.patientName) }}

                                </div>

                                <div>

                                    <div class="font-semibold text-[var(--primary)]">

                                        {{ patient.patientName }}

                                    </div>

                                    <div class="text-sm text-gray-500">

                                        Patient ID : {{ patient.patientId }}

                                    </div>

                                </div>

                            </div>

                        </td>

                        <!-- Disposition -->

                        <td>

                            <span
                                class="inline-flex px-3 py-1 rounded-full text-xs font-semibold bg-blue-100 text-blue-700"
                            >

                                {{ patient.disposition }}

                            </span>

                        </td>

                        <!-- Status -->

                        <td>

                            <span
                                class="inline-flex px-3 py-1 rounded-full text-xs font-semibold"
                                :style="statusStyle(patient.status)"
                            >

                                {{ patient.status }}

                            </span>

                        </td>

                        <!-- Actions -->

                        <td>

                            <div class="flex justify-center">

                                <button
                                    @click="viewPatient(patient)"
                                    class="px-5 py-2 rounded-xl border font-medium transition"
                                    style="
                                        border-color: var(--primary);
                                        color: var(--primary);
                                    "
                                >

                                    View

                                </button>

                            </div>

                        </td>

                    </tr>

                </tbody>

            </table>

            </div>
            <div
    class="flex items-center justify-between px-6 py-4 border-t">

    <p class="text-sm text-gray-500">

        Showing

        {{ (currentPage - 1) * pageSize + 1 }}

        -

        {{ Math.min(currentPage * pageSize, filteredPatients.length) }}

        of

        {{ filteredPatients.length }}

        patients

    </p>

    <div class="flex gap-2">

        <button

            class="px-4 py-2 rounded-lg border disabled:opacity-50"

            :disabled="currentPage === 1"

            @click="currentPage--">

            Previous

        </button>

        <button

            v-for="page in totalPages"

            :key="page"

            @click="currentPage = page"

            class="w-10 h-10 rounded-lg"

            :class="

                currentPage === page

                ? 'bg-[var(--primary)] text-white'

                : 'border'

            ">

            {{ page }}

        </button>

        <button

            class="px-4 py-2 rounded-lg border disabled:opacity-50"

            :disabled="currentPage === totalPages"

            @click="currentPage++">

            Next

        </button>

    </div>

</div>
            <!-- Overlay -->
            <div
                v-if="showPatientDrawer"
                class="fixed inset-0 bg-black/30 z-40"
                @click="showPatientDrawer = false">
            </div>

<!-- Drawer -->
<div
    class="fixed top-0 right-0 h-full w-[520px] bg-white shadow-2xl z-50 transition-transform duration-300 overflow-y-auto"
    :class="showPatientDrawer ? 'translate-x-0' : 'translate-x-full'"
>

    <!-- Header -->
    <div class="flex items-center justify-between px-6 py-5 border-b">

        <h2 class="text-2xl font-bold text-[var(--primary)]">

            Patient Details

        </h2>

        <button
            @click="showPatientDrawer = false"
            class="text-3xl font-light">

            ×

        </button>

    </div>

    <div
        v-if="patientDetails"
        class="p-6 space-y-6">

        <!-- Patient Information -->
        <div class="grid grid-cols-2 gap-4">

            <div>
                <p class="text-sm text-gray-500">Patient Name</p>
                <p class="font-semibold">
                    {{ patientDetails.patientName }}
                </p>
            </div>

            <div>
                <p class="text-sm text-gray-500">Patient ID</p>
                <p class="font-semibold">
                    {{ patientDetails.patientId }}
                </p>
            </div>

            <div>
                <p class="text-sm text-gray-500">Department</p>
                <p class="font-semibold">
                    {{ patientDetails.departmentName }}
                </p>
            </div>

            <div>
                <p class="text-sm text-gray-500">Physician</p>
                <p class="font-semibold">
                    {{ patientDetails.clinicianName }}
                </p>
            </div>

        </div>

        <hr>

        <!-- Decision Details -->

        <div class="space-y-4">

            <div>

                <p class="text-sm text-gray-500">

                    Disposition Type

                </p>

                <p class="font-semibold">

                    {{ patientDetails.dispositionTypeName }}

                </p>

            </div>

            <div>

                <p class="text-sm text-gray-500">

                    Decision Date

                </p>

                <p class="font-semibold">

                    {{ patientDetails.decisionDate }}

                </p>

            </div>

            <div>

                <p class="text-sm text-gray-500">

                    Expected Transition Date

                </p>

                <p class="font-semibold">

                    {{ patientDetails.expectedTransitionDate }}

                </p>

            </div>

            <div>

                <p class="text-sm text-gray-500">

                    Status

                </p>

                <span
                    class="inline-flex rounded-full px-3 py-1 text-sm font-semibold"
                    :class="{
                        'bg-yellow-100 text-yellow-700': patientDetails.status === 'Pending',
                        'bg-green-100 text-green-700': patientDetails.status === 'Approved',
                        'bg-red-100 text-red-700': patientDetails.status === 'Rejected'
                    }"
                >
                    {{ patientDetails.status }}
                </span>

            </div>

            <div>

                <p class="text-sm text-gray-500">

                    Clinical Notes

                </p>

                <div
                    class="mt-2 rounded-lg border bg-gray-50 p-4">

                    {{ patientDetails.notes }}

                </div>

            </div>

        </div>

    </div>

</div>

        </div>
  </HospitalLayout>
</template>

<script setup>

import HospitalLayout from "@/layouts/HospitalLayout.vue";
import { ref, computed, onMounted, watch } from "vue";

const search = ref("");

import * as physicianService
from "@/services/physicianService";

const patients = ref([]);
const showPatientDrawer = ref(false);

const patientDetails = ref(null);
const currentPage = ref(1);

const pageSize = 10;

onMounted(async () => {

     
    const response =
        await physicianService.getAssignedPatients();
        console.log(response.data);
    patients.value =
        response.data.data;
        return patients.value;

});

let debounceTimer;

watch(search, (value) => {

    clearTimeout(debounceTimer);

    debounceTimer = setTimeout(() => {

        loadPatients(value);

    }, 500);

});

const filteredPatients = computed(() => {

    return patients.value.filter(patient => {

        const name = patient.patientName || "";

        return name
            .toLowerCase()
            .includes(search.value.toLowerCase());

    });

});

const totalPages = computed(() => {

    return Math.ceil(
        filteredPatients.value.length / pageSize
    );

});

const paginatedPatients = computed(() => {

    const start =
        (currentPage.value - 1) * pageSize;

    return filteredPatients.value.slice(

        start,

        start + pageSize

    );

});

function statusStyle(status){

    switch(status){

        case "Pending":

            return {
                background:"rgba(217,119,6,.15)",
                color:"var(--warning)"
            };

        case "Approved":

            return {
                background:"rgba(22,163,74,.15)",
                color:"var(--success)"
            };

        case "Completed":

            return {
                background:"rgba(102,155,188,.15)",
                color:"var(--secondary)"
            };

        default:

            return {
                background:"#eee",
                color:"#555"
            };

    }

}

async function viewPatient(patient) {

    try {

        const response =
            await physicianService
                .getPatientDetails(patient.patientId);

        patientDetails.value =
            response.data.data;

        showPatientDrawer.value = true;

    }

    catch (error) {

        console.error(error);

    }

}

function getInitials(name) {

    if (!name) return "";

    return name
        .split(" ")
        .map(word => word[0])
        .join("")
        .toUpperCase();

}

</script>