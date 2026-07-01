<script setup>
import { ref, watch } from 'vue'

import AppButton from '@/components/common/AppButton.vue'

import { useCareManagerStore }
from '@/stores/careManager'

const props = defineProps({

    show: Boolean,

    patient: Object

})

const emit = defineEmits([

    'close',

    'created'

])

const careManager =
    useCareManagerStore()

const loading = ref(false)

const error = ref('')

const selectedReason = ref(null)

watch(

    () => props.show,

    async (value) => {

        if (!value)
            return

        error.value = ''

        selectedReason.value = null

        if (
            careManager.delayReasons.length === 0
        ) {
            await careManager.loadDelayReasons()
        }
        if (
            props.patient?.hasActiveDelay
        ) {

            selectedReason.value =
                props.patient.delayReasonId

        }

    }

)

async function submit() {

    error.value = ''

    if (!selectedReason.value) {

        error.value =
            'Please select a delay reason.'

        return

    }

    try {

        loading.value = true

        await careManager.createPatientDelay({

            patientId:
                props.patient.patientId,

            delayReasonId:
                selectedReason.value

        })

        emit('created')

    }

    catch (err) {

        error.value =
            err.response?.data?.message
            ??
            'Unable to report delay.'

    }

    finally {

        loading.value = false

    }

}
</script>

<template>

<div
    v-if="show"
    class="
    fixed
    inset-0
    z-50
    flex
    items-center
    justify-center
    bg-black/40">

    <div
        class="
        w-full
        max-w-lg
        rounded-3xl
        bg-white
        p-8
        shadow-2xl">

        <h2
            class="
            text-2xl
            font-bold
            mb-6">

            Report Patient Delay

        </h2>

        <div class="space-y-5">

            <div>

                <label class="font-medium">

                    Patient

                </label>

                <div
                    class="
                    mt-2
                    rounded-xl
                    border
                    p-3">

                    {{ patient.patientName }}

                </div>

            </div>

            <div>

                <label class="font-medium">

                    Delay Reason

                </label>

                <select

                    v-model="selectedReason"

                    class="
                    mt-2
                    w-full
                    rounded-xl
                    border
                    p-3">

                    <option
                        :value="null">

                        Select Delay Reason

                    </option>

                    <option

                        v-for="reason in careManager.delayReasons"

                        :key="reason.id"

                        :value="reason.id">

                        {{ reason.reasonName }}

                    </option>

                </select>

            </div>

            <div

                v-if="error"

                class="
                rounded-xl
                bg-red-100
                p-3
                text-red-600">

                {{ error }}

            </div>

            <div
                class="
                flex
                justify-end
                gap-3">

                <button

                    @click="$emit('close')"

                    class="
                    rounded-xl
                    border
                    px-5
                    py-2">

                    Cancel

                </button>

                <AppButton

                    :loading="loading"

                    @click="submit">

                    Report Delay

                </AppButton>

            </div>

        </div>

    </div>

</div>

</template>