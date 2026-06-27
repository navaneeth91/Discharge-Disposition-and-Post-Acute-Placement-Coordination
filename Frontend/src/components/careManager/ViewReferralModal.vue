<script setup>
import { ref, watch } from 'vue'

import AppButton from '@/components/common/AppButton.vue'

import {
    getReferralByPatient
} from '@/services/careManagerService'

const props = defineProps({

    show: {
        type: Boolean,
        default: false
    },

    patient: {
        type: Object,
        default: null
    }

})

const emit = defineEmits([
    'close'
])

const loading =
    ref(false)

const error =
    ref('')

const referral =
    ref(null)

watch(

    () => [props.show, props.patient],

    async ([show, patient]) => {

        if (
            !show ||
            !patient
        )
            return

        await loadReferral()

    },

    {
        immediate: true
    }

)

async function loadReferral() {

    try {

        loading.value = true

        error.value = ''

        referral.value = null

        const response =
            await getReferralByPatient(
                props.patient.patientId
            )

        if (
            response.data.data &&
            response.data.data.length > 0
        ) {

            referral.value =
                response.data.data[0]

        }

        else {

            error.value =
                'Referral not found.'

        }

    }

    catch (err) {

        console.error(err)

        error.value =
            err.response?.data?.message ||
            'Failed to load referral details.'

    }

    finally {

        loading.value = false

    }

}

function close() {

    referral.value = null

    error.value = ''

    emit('close')

}

function formatDate(date) {

    if (!date)
        return '-'

    return new Date(date)
        .toLocaleString()

}
</script>
<template>

<Teleport to="body">

    <div
        v-if="show"
        class="
        fixed
        inset-0
        z-50
        flex
        items-center
        justify-center
        bg-black/50
        backdrop-blur-sm">

        <div
            class="
            w-full
            max-w-2xl
            rounded-3xl
            bg-white
            shadow-2xl
            overflow-hidden">

            <!-- Header -->

            <div
                class="
                flex
                items-center
                justify-between
                px-8
                py-6"
                style="
                background:var(--primary);
                color:white;">

                <div>

                    <h2
                        class="
                        text-2xl
                        font-bold">

                        Referral Details

                    </h2>

                    <p
                        class="
                        text-sm
                        opacity-80">

                        View referral information

                    </p>

                </div>

                <button
                    @click="close"
                    class="
                    text-3xl
                    leading-none
                    hover:opacity-70">

                    &times;

                </button>

            </div>

            <!-- Body -->

            <div
                class="
                p-8
                space-y-6">

                <!-- Loading -->

                <div
                    v-if="loading"
                    class="
                    py-10
                    text-center">

                    Loading referral...

                </div>

                <!-- Error -->

                <div
                    v-else-if="error"
                    class="
                    rounded-xl
                    bg-red-50
                    p-4
                    text-red-600">

                    {{ error }}

                </div>

                <!-- Referral Details -->

                <div
                    v-else-if="referral"
                    class="space-y-6">

                    <div
                        class="
                        rounded-2xl
                        border
                        p-5
                        grid
                        grid-cols-2
                        gap-6">

                        <div>

                            <p class="text-sm text-slate-500">

                                Patient

                            </p>

                            <p class="font-semibold">

                                {{ referral.patientName }}

                            </p>

                        </div>

                        <div>

                            <p class="text-sm text-slate-500">

                                Provider

                            </p>

                            <p class="font-semibold">

                                {{ referral.providerName }}

                            </p>

                        </div>

                        <div>

                            <p class="text-sm text-slate-500">

                                Care Manager

                            </p>

                            <p class="font-semibold">

                                {{ referral.careManagerName }}

                            </p>

                        </div>

                        <div>

                            <p class="text-sm text-slate-500">

                                Created On

                            </p>

                            <p class="font-semibold">

                                {{ formatDate(referral.createdDate) }}

                            </p>

                        </div>

                    </div>

                    <div
                        class="
                        rounded-2xl
                        border
                        p-5
                        grid
                        grid-cols-2
                        gap-6">

                        <div>

                            <p class="text-sm text-slate-500">

                                Priority

                            </p>

                            <span
                                class="
                                inline-flex
                                rounded-full
                                px-4
                                py-1
                                text-sm
                                font-semibold
                                bg-orange-100
                                text-orange-700">

                                {{ referral.priority }}

                            </span>

                        </div>

                        <div>

                            <p class="text-sm text-slate-500">

                                Status

                            </p>

                            <span
                                class="
                                inline-flex
                                rounded-full
                                px-4
                                py-1
                                text-sm
                                font-semibold
                                bg-blue-100
                                text-blue-700">

                                {{ referral.status }}

                            </span>

                        </div>

                    </div>

                </div>

            </div>

            <!-- Footer -->

            <div
                class="
                flex
                justify-end
                border-t
                px-8
                py-6">

                <div
                    class="w-40">

                    <AppButton
                        @click="close">

                        Close

                    </AppButton>

                </div>

            </div>

        </div>

    </div>

</Teleport>

</template>

<style scoped>

.border{

    border-color:var(--border);

}

</style>