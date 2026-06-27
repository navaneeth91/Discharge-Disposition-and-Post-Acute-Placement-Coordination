<script setup>
import { ref, watch } from 'vue'

import { useAuthStore } from '@/stores/auth'

import AppButton from '@/components/common/AppButton.vue'

import {
    getProvidersByDisposition,
    createReferral
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
    'close',
    'created'
])

const auth =
    useAuthStore()

const providers =
    ref([])

const selectedProvider =
    ref('')

const priority =
    ref(0)

const loading =
    ref(false)

const loadingProviders =
    ref(false)

const error =
    ref('')

const priorities = [

    {
        value: 0,
        label: 'Normal'
    },

    {
        value: 1,
        label: 'Low'
    },

    {
        value: 2,
        label: 'High'
    },

    {
        value: 3,
        label: 'Critical'
    }

]

watch(
    () => [props.show, props.patient],
    async ([show, patient]) => {

        if (!show || !patient)
            return

        await loadProviders()

    },
    {
        immediate: true
    }
)

async function loadProviders() {

    console.log('Patient:', props.patient)

    console.log(
        'DispositionTypeId:',
        props.patient?.dispositionTypeId
    )

    if (!props.patient?.dispositionTypeId)
        return

    try {

        loadingProviders.value = true

        error.value = ''

        const response =
            await getProvidersByDisposition(
                props.patient.dispositionTypeId
            )

        console.log('API Response:', response.data)

        providers.value =
            response.data.data

        console.log('Providers:', providers.value)

    }
    catch (err) {

        console.error(err)

        error.value =
            err.response?.data?.message ||
            'Failed to load providers.'

    }
    finally {

        loadingProviders.value = false

    }

}

async function submitReferral() {

    if (!selectedProvider.value) {

        error.value =
            'Please select a provider.'

        return

    }

    try {

        loading.value = true

        error.value = ''

        await createReferral({

            patientId:
                props.patient.patientId,

            providerId:
                Number(
                    selectedProvider.value
                ),

            careManagerId:
                Number(
                    auth.userId
                ),

            status: 1,

            priority:
                Number(
                    priority.value
                )

        })

        emit('created')

        close()

    }

    catch (err) {

        error.value =
            err.response?.data?.message ||
            'Failed to create referral.'

    }

    finally {

        loading.value = false

    }

}

function close() {

    selectedProvider.value = ''

    priority.value = 0

    providers.value = []

    error.value = ''

    emit('close')

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

                        Create Referral

                    </h2>

                    <p
                        class="
                        text-sm
                        opacity-80">

                        Refer patient to a Post Acute Provider

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
                space-y-8">

                <!-- Patient Details -->

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

                            {{ patient.patientName }}

                        </p>

                    </div>

                    <div>

                        <p class="text-sm text-slate-500">

                            MRN

                        </p>

                        <p class="font-semibold">

                            {{ patient.mrn }}

                        </p>

                    </div>

                    <div>

                        <p class="text-sm text-slate-500">

                            Department

                        </p>

                        <p class="font-semibold">

                            {{ patient.departmentName }}

                        </p>

                    </div>

                    <div>

                        <p class="text-sm text-slate-500">

                            Disposition

                        </p>

                        <p class="font-semibold">

                            {{ patient.dispositionType }}

                        </p>

                    </div>

                </div>

                <!-- Provider -->

                <div>

                    <label
                        class="
                        block
                        mb-2
                        text-sm
                        font-semibold">

                        Provider

                    </label>

                    <select

                        v-model="selectedProvider"

                        :disabled="loadingProviders"

                        class="
                        w-full
                        rounded-xl
                        border
                        px-4
                        py-3
                        outline-none">

                        <option value="">

                            Select Provider

                        </option>

                        <option

                            v-for="provider in providers"

                            :key="provider.providerId"

                            :value="provider.providerId">

                            {{ provider.providerName }}

                            -
                            {{ provider.city }}

                        </option>

                    </select>

                </div>

                <!-- Priority -->

                <div>

                    <label
                        class="
                        block
                        mb-2
                        text-sm
                        font-semibold">

                        Priority

                    </label>

                    <select

                        v-model="priority"

                        class="
                        w-full
                        rounded-xl
                        border
                        px-4
                        py-3
                        outline-none">

                        <option

                            v-for="item in priorities"

                            :key="item.value"

                            :value="item.value">

                            {{ item.label }}

                        </option>

                    </select>

                </div>

                <!-- Error -->

                <div

                    v-if="error"

                    class="
                    rounded-xl
                    bg-red-50
                    p-4
                    text-red-600">

                    {{ error }}

                </div>

            </div>

            <!-- Footer -->

            <div
                class="
                flex
                justify-end
                gap-4
                px-8
                py-6
                border-t">

                <button

                    @click="close"

                    class="
                    rounded-xl
                    border
                    px-6
                    py-3
                    font-semibold">

                    Cancel

                </button>

                <div class="w-48">

                    <AppButton

                        :loading="loading"

                        @click="submitReferral">

                        Create Referral

                    </AppButton>

                </div>

            </div>

        </div>

    </div>

</Teleport>

</template>

<style scoped>

select{

    border-color:var(--border);

    transition:.25s;

}

select:focus{

    border-color:#003049;

    box-shadow:
        0 0 0 4px
        rgba(102,155,188,.2);

}

</style>