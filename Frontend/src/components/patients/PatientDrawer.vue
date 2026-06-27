<script setup>
import { ref }
from 'vue'

const props =
    defineProps({

        patient: Object,

        show: Boolean
    })

const emit =
    defineEmits([
        'close',
        'discharge'
    ])

const dischargeDate =
    ref(
        new Date()
            .toISOString()
            .split('T')[0]
    )
</script>

<template>

<Transition name="slide">

<div
    v-if="show"
    class="
    fixed
    inset-0
    bg-black/40
    z-50
    flex
    justify-end">

    <div
        class="
        w-[550px]
        h-full
        bg-white
        shadow-2xl
        overflow-y-auto
        p-8">

        <div
            class="
            flex
            justify-between
            mb-8">

            <h2
                class="
                text-3xl
                font-bold
                text-[#003049]">

                Patient Details

            </h2>

            <button
                @click="
                $emit('close')"

                class="
                text-2xl">

                ✕

            </button>

        </div>

        <div
            v-if="patient"
            class="space-y-6">

            <div>

                <h3
                    class="
                    text-2xl
                    font-bold">

                    {{
                        patient.firstName
                    }}
                    {{
                        patient.lastName
                    }}

                </h3>

                <p
                    class="
                    text-slate-500">

                    MRN:
                    {{
                        patient.mrn
                    }}

                </p>

            </div>

            <div class="grid grid-cols-2 gap-6">

                <div>

                    <p class="text-slate-500">
                        Gender
                    </p>

                    <h4>
                        {{
                            patient.gender
                        }}
                    </h4>

                </div>

                <div>

                    <p class="text-slate-500">
                        Department
                    </p>

                    <h4>
                        {{
                            patient.departmentName
                        }}
                    </h4>

                </div>

                <div>

                    <p class="text-slate-500">
                        Email
                    </p>

                    <h4>
                        {{
                            patient.email
                        }}
                    </h4>

                </div>

                <div>

                    <p class="text-slate-500">
                        Phone
                    </p>

                    <h4>
                        {{
                            patient.phoneNumber
                        }}
                    </h4>

                </div>

                <div>

                    <p class="text-slate-500">
                        Admission
                    </p>

                    <h4>

                        {{
                            new Date(
                                patient.admissionDate
                            )
                            .toLocaleDateString()
                        }}

                    </h4>

                </div>

                <div>

                    <p class="text-slate-500">
                        Expected Discharge
                    </p>

                    <h4>

                        {{
                            patient.expectedDischargeDate
                        }}

                    </h4>

                </div>

            </div>

            <div>

                <p class="text-slate-500 mb-3">

                    Status

                </p>

                <span
                    v-if="
                    patient.isActive"

                    class="
                    px-4
                    py-2
                    rounded-full
                    bg-green-100
                    text-green-700">

                    Active

                </span>

                <span
                    v-else

                    class="
                    px-4
                    py-2
                    rounded-full
                    bg-red-100
                    text-red-700">

                    Discharged

                </span>

            </div>

            <div
                v-if="
                patient.actualDischargeDate">

                <p class="text-slate-500">

                    Actual Discharge

                </p>

                <h4>

                    {{
                        new Date(
                            patient.actualDischargeDate
                        )
                        .toLocaleDateString()
                    }}

                </h4>

            </div>

            <div
                v-if="
                patient.isActive"

                class="pt-6">

                <label
                    class="
                    block
                    mb-2
                    font-medium">

                    Discharge Date

                </label>

                <input
                    v-model="dischargeDate"
                    type="date"
                    class="
                    w-full
                    border
                    rounded-xl
                    p-3">

                <button
                    @click="
                    emit(
                        'discharge',
                        dischargeDate
                    )"

                    class="
                    w-full
                    mt-6
                    py-3
                    rounded-xl
                    text-white
                    bg-[#003049]
                    hover:bg-[#00243A]
                    transition">

                    Discharge Patient

                </button>

            </div>

        </div>

    </div>

</div>

</Transition>

</template>

<style scoped>
.slide-enter-active,
.slide-leave-active {
    transition: .3s;
}

.slide-enter-from,
.slide-leave-to {
    transform: translateX(100%);
}
</style>