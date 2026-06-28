<template>
    <HospitalLayout>
        <div class="bg-white rounded-2xl shadow-md border border-[var(--border)]">

    <!-- Header -->
    <div
      class="flex justify-between items-center px-6 py-5 border-b border-[var(--border)]"
    >
      <div>
        <h2 class="text-xl font-bold text-[var(--primary)]">
          Department Patients
        </h2>

        <p class="text-sm text-[var(--text-secondary)]">
          Patients available for disposition decision
        </p>
      </div>

      <input
        v-model="search"
        type="text"
        placeholder="Search patient..."
        class="w-72 rounded-lg border border-[var(--border)] px-4 py-2 focus:outline-none focus:ring-2 focus:ring-[var(--secondary)]"
      />
    </div>

    <!-- Table -->

    <div class="overflow-x-auto">

      <table class="min-w-full">

        <thead class="bg-gray-50 border-b">

    <tr>

        <th class="px-5 py-4 text-left text-sm font-semibold text-[var(--primary)]">
            Patient
        </th>

        <th class="px-5 py-4 text-left text-sm font-semibold text-[var(--primary)]">
            DOB
        </th>

        <th class="px-5 py-4 text-left text-sm font-semibold text-[var(--primary)]">
            Status
        </th>

        <th class="px-5 py-4 text-center text-sm font-semibold text-[var(--primary)]">
            Actions
        </th>

    </tr>

</thead>

        <tbody>

  <tr
    v-for="patient in paginatedPatients"
    :key="patient.patientId"
    class="border-b border-gray-200 hover:bg-gray-50 transition-colors"
  >

    <!-- Patient -->
    <td class="px-5 py-4">

      <div class="flex items-center gap-4">

        <!-- Avatar -->
        <div
          class="w-12 h-12 rounded-full bg-[var(--primary)] text-white flex items-center justify-center font-bold text-sm shrink-0"
        >
          {{ getInitials(patient.patientName) }}
        </div>

        <!-- Patient Name + Subtitle -->
        <div>

          <h6 class="font-semibold text-[var(--primary)]">

            {{ patient.patientName }}

          </h6>

          <p class="text-sm text-gray-500">

            Patient ID : {{ patient.patientId }}

          </p>

        </div>

      </div>

    </td>

    <!-- DOB -->
    <td class="px-5 py-4 text-gray-600">

      {{ patient.dob }}

    </td>

    <!-- Status -->
    <td class="px-5 py-4">

    <span
        class="inline-flex px-3 py-1 rounded-full text-xs font-semibold"
        :class="patient.status == 1
            ? 'bg-green-100 text-green-700'
            : 'bg-red-100 text-red-700'">

        {{ patient.status == 1 ? 'Active' : 'Inactive' }}

    </span>

</td>

    <!-- Actions -->
    <td class="px-5 py-4 text-center">

      <button
        class="px-5 py-2 rounded-xl bg-[var(--primary)] text-white font-medium hover:bg-[var(--primary-hover)] transition"
        @click="createDecision(patient)"
      >

        Create Decision

      </button>

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

    <div class="flex items-center gap-2">

    <!-- Previous -->

    <button
        class="px-4 py-2 rounded-lg border disabled:opacity-50"
        :disabled="currentPage === 1"
        @click="currentPage--">

        Previous

    </button>

    <!-- First Page -->

    <button
        @click="currentPage = 1"
        class="w-10 h-10 rounded-lg"
        :class="buttonClass(1)">

        1

    </button>

    <!-- Left Dots -->

    <span
        v-if="showLeftDots"
        class="px-2 text-gray-500">

        ...

    </span>

    <!-- Middle Pages -->

    <button
        v-for="page in middlePages"
        :key="page"
        @click="currentPage = page"
        class="w-10 h-10 rounded-lg"
        :class="buttonClass(page)">

        {{ page }}

    </button>

    <!-- Right Dots -->

    <span
        v-if="showRightDots"
        class="px-2 text-gray-500">

        ...

    </span>

    <!-- Last Page -->

    <button
        v-if="totalPages > 1"
        @click="currentPage = totalPages"
        class="w-10 h-10 rounded-lg"
        :class="buttonClass(totalPages)">

        {{ totalPages }}

    </button>

    <!-- Next -->

    <button
        class="px-4 py-2 rounded-lg border disabled:opacity-50"
        :disabled="currentPage === totalPages"
        @click="currentPage++">

        Next

    </button>

</div>

</div>

    <!-- Drawer Backdrop -->

    <div
        v-if="showDrawer"
        class="fixed inset-0 bg-black/30 z-40"
        @click="showDrawer=false">
    </div>

<!-- Right Drawer -->

<div
    class="fixed top-0 right-0 h-full w-[480px] bg-white shadow-2xl z-50 transition-all duration-300"
    :class="showDrawer ? 'translate-x-0' : 'translate-x-full'"
>

    <div
        class="flex justify-between items-center px-6 py-5 border-b">

        <h2
            class="text-xl font-bold text-[var(--primary)]">

            Create Disposition Decision

        </h2>

        <button
            @click="showDrawer=false"
            class="text-2xl">

            ×

        </button>

    </div>

    <div class="p-6 space-y-5">

        <div>

            <label
                class="font-semibold">

                Patient

            </label>

            <input

                :value="selectedPatient?.patientName"

                disabled

                class="w-full mt-2 border rounded-lg px-3 py-2 bg-gray-100"/>

        </div>

        <div>

            <label
                class="font-semibold">

                Disposition Type

            </label>

            <select
    v-model="decision.dispositionTypeId"
    class="w-full mt-2 border rounded-lg px-3 py-2"
>

    <option value="">

        Select Disposition

    </option>

    <option value="1">

        Home Health

    </option>

    <option value="2">

        Skilled Nursing Facility

    </option>

    <option value="3">

        Inpatient Rehabilitation

    </option>

    <option value="4">

        Long-Term Acute Care Hospital

    </option>

    <option value="5">

        Hospice

    </option>

    <option value="6">

        Assisted Living

    </option>

    <option value="7">

        Home with Family Support

    </option>

    <option value="8">

        Outpatient Therapy

    </option>

</select>

        </div>

        <div>

            <label
                class="font-semibold">

                Expected Transition Date

            </label>

            <input

                type="date"

                v-model="decision.expectedTransitionDate"

                class="w-full mt-2 border rounded-lg px-3 py-2"/>

        </div>

        <div>

            <label
                class="font-semibold">

                Notes

            </label>

            <textarea

                rows="5"

                v-model="decision.notes"

                class="w-full mt-2 border rounded-lg px-3 py-2">

            </textarea>

        </div>

        <div
            class="flex justify-end gap-3 pt-5">

            <button

                @click="showDrawer=false"

                class="px-5 py-2 rounded-lg border">

                Cancel

            </button>

            <button

                @click="saveDecision"

                class="px-5 py-2 rounded-lg bg-[var(--primary)] text-white">

                Save Decision

            </button>

        </div>

    </div>

</div>

  </div>
    </HospitalLayout>
  
</template>

<script setup>

import { ref, computed, onMounted, reactive, watch} from "vue";

import { useRouter } from "vue-router";
import { useToast } from 'vue-toastification'
import * as physicianService
from "@/services/physicianService";
import HospitalLayout from "@/layouts/HospitalLayout.vue";

const router = useRouter();
const patients = ref([]);
const search = ref("");
const toast = useToast()

const showDrawer = ref(false);

const selectedPatient = ref(null);
const currentPage = ref(1);

const pageSize = 10;


const decision = reactive({

    patientId: null,

    dispositionTypeId: "",

    clinicianId: "",

    departmentId: "",

    expectedTransitionDate: "",

    notes: ""

});

let debounceTimer;

watch(search, (value) => {

    clearTimeout(debounceTimer);

    debounceTimer = setTimeout(() => {

        loadPatients(value);

    }, 500);

});

onMounted(async () => {

    const response =
        await physicianService.getPatientsByDepartment();

    patients.value =
        response.data.data;

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

function statusClass(status){

    if(status==="Pending")
        return "bg-yellow-100 text-yellow-700";

    if(status==="Approved")
        return "bg-green-100 text-green-700";

    return "bg-blue-100 text-blue-700";

}


function createDecision(patient){

    console.log(patient);
    selectedPatient.value = patient;

    decision.patientId = patient.patientId;

    decision.departmentId = patient.deptId;

    decision.clinicianId = Number(sessionStorage.getItem("userId"));

    showDrawer.value = true;

}

async function saveDecision(){

    try{
        console.log(decision);

        decision.clinicianId =
            sessionStorage.getItem("userId");

        // decision.departmentId =
        //     sessionStorage.getItem("departmentId");

        await physicianService
            .createDispositionDecision(decision);

        toast.success("Disposition Decision Created Successfully");

        showDrawer.value = false;

    }

    catch(error){
        console.log(error);

    }

}



const showLeftDots = computed(() => {

    return currentPage.value > 3;

});

const showRightDots = computed(() => {

    return currentPage.value < totalPages.value - 2;

});

const middlePages = computed(() => {

    const pages = [];

    const start = Math.max(2, currentPage.value - 1);

    const end = Math.min(
        totalPages.value - 1,
        currentPage.value + 1
    );

    for (let i = start; i <= end; i++) {

        pages.push(i);

    }

    return pages;

});
function buttonClass(page) {

    return currentPage.value === page

        ? "bg-[var(--primary)] text-white"

        : "border";

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
