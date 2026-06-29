<script setup>
import { ref } from 'vue'

import AppButton from '@/components/common/AppButton.vue'

import {
    createAuthorization
} from '@/services/careManagerService'

const props = defineProps({

    show: {
        type: Boolean,
        default: false
    },

    referral: {
        type: Object,
        default: null
    }

})

const emit = defineEmits([

    'close',

    'created'

])

const payerId =
    ref('')

const memberId =
    ref('')

const serviceType =
    ref('')

const requestingOrganization =
    ref('')

const loading =
    ref(false)

const error =
    ref('')

async function submitAuthorization() {

    if (
        !payerId.value ||
        !memberId.value ||
        !serviceType.value ||
        !requestingOrganization.value
    ) {

        error.value =
            'Please fill all fields.'

        return

    }

    try {

        loading.value = true

        error.value = ''

        await createAuthorization({

            patientId:
                props.referral.patientId,

            referralId:
                props.referral.referralId,

            payerId:
                Number(
                    payerId.value
                ),

            memberId:
                Number(
                    memberId.value
                ),

            serviceType:
                serviceType.value,

            requestingOrganization:
                requestingOrganization.value

        })

        emit('created')

        close()

    }

    catch (err) {

        error.value =
            err.response?.data?.message ||
            'Failed to create authorization.'

    }

    finally {

        loading.value = false

    }

}

function close() {

    payerId.value = ''

    memberId.value = ''

    serviceType.value = ''

    requestingOrganization.value = ''

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

                        Create Authorization

                    </h2>

                    <p
                        class="
                        text-sm
                        opacity-80">

                        Submit an insurance authorization request.

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

                <!-- Referral Details -->

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

                            Referral ID

                        </p>

                        <p class="font-semibold">

                            {{ referral.referralId }}

                        </p>

                    </div>

                </div>

                <!-- Form -->

                <div class="grid grid-cols-2 gap-6">

                    <div>

                        <label
                            class="
                            block
                            mb-2
                            text-sm
                            font-semibold">

                            Payer ID

                        </label>

                        <input

                            v-model="payerId"

                            type="number"

                            class="
                            w-full
                            rounded-xl
                            border
                            px-4
                            py-3
                            outline-none"

                            placeholder="Enter Payer ID" />

                    </div>

                    <div>

                        <label
                            class="
                            block
                            mb-2
                            text-sm
                            font-semibold">

                            Member ID

                        </label>

                        <input

                            v-model="memberId"

                            type="number"

                            class="
                            w-full
                            rounded-xl
                            border
                            px-4
                            py-3
                            outline-none"

                            placeholder="Enter Member ID" />

                    </div>

                    <div>

                        <label
                            class="
                            block
                            mb-2
                            text-sm
                            font-semibold">

                            Service Type

                        </label>

                        <input

                            v-model="serviceType"

                            class="
                            w-full
                            rounded-xl
                            border
                            px-4
                            py-3
                            outline-none"

                            placeholder="e.g. Skilled Nursing Facility" />

                    </div>

                    <div>

                        <label
                            class="
                            block
                            mb-2
                            text-sm
                            font-semibold">

                            Requesting Organization

                        </label>

                        <input

                            v-model="requestingOrganization"

                            class="
                            w-full
                            rounded-xl
                            border
                            px-4
                            py-3
                            outline-none"

                            placeholder="Enter Organization" />

                    </div>

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

                <div class="w-56">

                    <AppButton

                        :loading="loading"

                        @click="submitAuthorization">

                        Create Authorization

                    </AppButton>

                </div>

            </div>

        </div>

    </div>

</Teleport>

</template>

<style scoped>

input{

    border-color:var(--border);

    transition:.25s;

}

input:focus{

    border-color:#003049;

    box-shadow:
        0 0 0 4px
        rgba(102,155,188,.20);

}

</style>