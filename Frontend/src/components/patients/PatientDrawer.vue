<script setup>
import { ref } from 'vue'

const props = defineProps({
    patient: Object,
    show: Boolean
})

const emit = defineEmits([
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
    bg-black/30
    z-50
    flex
    justify-end">

    <div
        class="
        w-[500px]
        h-full
        bg-white
        shadow-2xl
        p-8
        overflow-y-auto">

        <div
            class="
            flex
            justify-between
            mb-8">

            <h2
                class="
                text-3xl
                font-bold
                text-[#2D1E3E]">

                Patient Details

            </h2>

            <button
                @click="$emit('close')">

                ✕

            </button>

        </div>

        <div
            v-if="patient"
            class="space-y-4">

            <div>

                <p class="text-slate-500">
                    Name
                </p>

                <h3
                    class="text-xl font-semibold">

                    {{ patient.firstName }}
                    {{ patient.lastName }}

                </h3>

            </div>

            <div>

                <p class="text-slate-500">
                    Admission Date
                </p>

                <h3>

                    {{ patient.admissionDate }}

                </h3>

            </div>

            <div>

                <p class="text-slate-500">
                    Status
                </p>

                <span
                    v-if="patient.isActive"
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

            </div>

            <div
                v-if="patient.isActive"
                class="pt-8">

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
                    mt-6
                    w-full
                    py-3
                    rounded-xl
                    bg-[#614083]
                    text-white
                    hover:bg-[#53366F]
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
    transition: all .3s ease;
}

.slide-enter-from,
.slide-leave-to {
    transform: translateX(100%);
}
</style>