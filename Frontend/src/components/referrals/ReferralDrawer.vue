<script setup>
import { computed } from 'vue'
import { useAuthStore } from '@/stores/auth'

const auth = useAuthStore()
const props =
    defineProps({

        referral: Object,

        show: Boolean
    })

const emit =
    defineEmits([
        'close',
        'status'
    ])

const priorityClass =
    computed(() => {

        switch (
            props.referral?.priority
        ) {

            case 'Critical':
                return 'bg-red-100 text-red-700'

            case 'High':
                return 'bg-orange-100 text-orange-700'

            case 'Low':
                return 'bg-blue-100 text-blue-700'

            default:
                return 'bg-gray-100 text-gray-700'
        }
    })

const statusClass =
    computed(() => {

        switch (
            props.referral?.status
        ) {

            case 'Approved':
                return 'bg-green-100 text-green-700'

            case 'Denied':
                return 'bg-red-100 text-red-700'

            default:
                return 'bg-yellow-100 text-yellow-700'
        }
    })
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
                text-[#003049]">

                Referral Details

            </h2>

            <button
                @click="$emit('close')">

                ✕

            </button>

        </div>

        <div
            v-if="referral"

            class="
            space-y-6">

            <div>

                <p class="text-gray-500">
                    Patient
                </p>

                <h3
                    class="
                    text-xl
                    font-semibold">

                    {{ referral.patientName }}

                </h3>

            </div>

            <div>

                <p class="text-gray-500">
                    Provider
                </p>

                <h3>

                    {{ referral.providerName }}

                </h3>

            </div>

            <div>

                <p class="text-gray-500">
                    Care Manager
                </p>

                <h3>

                    {{ referral.careManagerName }}

                </h3>

            </div>

            <div>

                <p class="text-gray-500">
                    Priority
                </p>

                <span
                    :class="priorityClass"

                    class="
                    px-3
                    py-1
                    rounded-full">

                    {{ referral.priority }}

                </span>

            </div>

            <div>

                <p class="text-gray-500">
                    Status
                </p>

                <span
                    :class="statusClass"

                    class="
                    px-3
                    py-1
                    rounded-full">

                    {{ referral.status }}

                </span>

            </div>

            <div>

                <p class="text-gray-500">
                    Created Date
                </p>

                <h3>

                    {{
                        new Date(
                            referral.createdDate
                        )
                        .toLocaleDateString()
                    }}

                </h3>

            </div>

            <div
                v-if="
                referral.status === 'Pending'
                &&
                auth.role ==='Post-Acute Provider'"
                class="
                pt-8
                space-y-3">

                <button
                    @click="
                    emit(
                        'status',
                        'Approved'
                    )"

                    class="
                    w-full
                    py-3
                    rounded-xl
                    bg-green-600
                    text-white">

                    Approve

                </button>

                <button
                    @click="
                    emit(
                        'status',
                        'Denied'
                    )"

                    class="
                    w-full
                    py-3
                    rounded-xl
                    bg-red-600
                    text-white">

                    Deny

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