<script setup>
defineProps({

    show: Boolean,

    referral: Object

})

const emit = defineEmits([
    'close'
])
</script>

<template>

<div
    v-if="show && referral"
    class="
    fixed
    inset-0
    bg-black/50
    flex
    items-center
    justify-center
    z-50
    p-6">

    <div
    class="
    bg-white
    rounded-3xl
    shadow-xl
    w-full
    max-w-4xl
    max-h-[90vh]
    overflow-y-auto
    p-8">

        <!-- Header -->

        <div
            class="
            flex
            justify-between
            items-center
            mb-8">

            <h2
                class="
                text-2xl
                font-bold
                text-[#003049]">

                View Referral

            </h2>

            <button
                @click="emit('close')"
                class="
                text-gray-500
                hover:text-black
                text-2xl">

                ✕

            </button>

        </div>

        <!-- Referral Information -->

        <div
            class="
            border
            rounded-2xl
            p-6
            mb-6">

            <h3
                class="
                text-lg
                font-semibold
                text-[#003049]
                mb-4">

                Referral Information

            </h3>

            <div class="grid md:grid-cols-2 gap-6">

                <div>

                    <p class="text-gray-500">
                        Referral ID
                    </p>

                    <p class="font-semibold">
                        {{ referral.referralId }}
                    </p>

                </div>

                <div>

                    <p class="text-gray-500">
                        Referral Date
                    </p>

                    <p class="font-semibold">
                        {{ referral.referralDate }}
                    </p>

                </div>

                <div>

                    <p class="text-gray-500">
                        Status
                    </p>

                    <p class="font-semibold">
                        {{ referral.status }}
                    </p>

                </div>

                <div>

                    <p class="text-gray-500">
                        Priority
                    </p>

                    <p class="font-semibold">
                        {{ referral.priority }}
                    </p>

                </div>

            </div>

        </div>

        <!-- Patient Information -->

        <div
            class="
            border
            rounded-2xl
            p-6
            mb-6">

            <h3
                class="
                text-lg
                font-semibold
                text-[#003049]
                mb-4">

                Patient Information

            </h3>

            <div class="grid md:grid-cols-2 gap-6">

                <div>

                    <p class="text-gray-500">
                        Patient Name
                    </p>

                    <p class="font-semibold">
                        {{ referral.patientName }}
                    </p>

                </div>

                <div>

                    <p class="text-gray-500">
                        MRN
                    </p>

                    <p class="font-semibold">
                        {{ referral.mrn }}
                    </p>

                </div>

                <div>

                    <p class="text-gray-500">
                        Date of Birth
                    </p>

                    <p class="font-semibold">
                        {{ referral.dob }}
                    </p>

                </div>

                <div>

                    <p class="text-gray-500">
                        Gender
                    </p>

                    <p class="font-semibold">
                        {{ referral.gender }}
                    </p>

                </div>

                <div>

                    <p class="text-gray-500">
                        Department
                    </p>

                    <p class="font-semibold">
                        {{ referral.department }}
                    </p>

                </div>

                <div>

                    <p class="text-gray-500">
                        Expected Discharge
                    </p>

                    <p class="font-semibold">
                        {{ referral.expectedDischargeDate }}
                    </p>

                </div>

            </div>

        </div>

        <!-- Contact Information -->

        <div
            class="
            border
            rounded-2xl
            p-6
            mb-6">

            <h3
                class="
                text-lg
                font-semibold
                text-[#003049]
                mb-4">

                Contact Information

            </h3>

            <div class="grid md:grid-cols-2 gap-6">

                <div>

                    <p class="text-gray-500">
                        Email
                    </p>

                    <p class="font-semibold">
                        {{ referral.email || 'N/A' }}
                    </p>

                </div>

                <div>

                    <p class="text-gray-500">
                        Phone
                    </p>

                    <p class="font-semibold">
                        {{ referral.phoneNumber || 'N/A' }}
                    </p>

                </div>

            </div>

        </div>

        <!-- Care Team -->

        <div
            class="
            border
            rounded-2xl
            p-6">

            <h3
                class="
                text-lg
                font-semibold
                text-[#003049]
                mb-4">

                Care Team

            </h3>

            <div class="grid md:grid-cols-2 gap-6">

                <div>

                    <p class="text-gray-500">
                        Provider
                    </p>

                    <p class="font-semibold">
                        {{ referral.providerName }}
                    </p>

                </div>

                <div>

                    <p class="text-gray-500">
                        Care Manager
                    </p>

                    <p class="font-semibold">
                        {{ referral.careManager }}
                    </p>

                </div>

            </div>

        </div>

        <!-- Footer -->

        <div
            class="
            mt-8
            flex
            justify-end">

            <button
                @click="emit('close')"

                class="
                px-6
                py-3
                rounded-xl
                bg-[#003049]
                text-white
                hover:bg-[#669BBC]
                transition">

                Close

            </button>

        </div>

    </div>

</div>

</template>