<script setup>
import { ref } from 'vue'

import HospitalLayout
from '@/layouts/HospitalLayout.vue'

import AllReferralsTable
from '@/components/provider/AllReferralsTable.vue'

const searchTerm =
    ref('')

const statusFilter =
    ref('All')
</script>

<template>

<HospitalLayout>

    <div class="space-y-6 fade-up">

        <div
            class="
            bg-white
            rounded-3xl
            p-8
            shadow-lg">

            <!-- Header -->

            <div
                class="
                flex
                flex-col
                md:flex-row
                md:justify-between
                md:items-center
                gap-4
                mb-8">

                <div>

                    <h1
                        class="
                        text-3xl
                        font-bold
                        text-[#003049]">

                        Referrals

                    </h1>

                    <p
                        class="
                        text-gray-500
                        mt-1">

                        View and manage referrals

                    </p>

                </div>

                <!-- Search + Filter -->

                <div
                    class="
                    flex
                    flex-col
                    md:flex-row
                    gap-3">

                    <input
                        v-model="searchTerm"

                        placeholder="
                        Search referrals..."

                        class="
                        w-full
                        md:w-72
                        px-4
                        py-3
                        rounded-xl
                        border
                        border-gray-200
                        outline-none
                        focus:ring-4
                        focus:ring-blue-100">

                    <select
                        v-model="statusFilter"

                        class="
                        px-4
                        py-3
                        rounded-xl
                        border
                        border-gray-200
                        outline-none">

                        <option>
                            All
                        </option>

                        <option>
                            Pending
                        </option>

                        <option>
                            Approved
                        </option>

                        <option>
                            Denied
                        </option>

                    </select>

                </div>

            </div>

            <AllReferralsTable
            title="All Referrals"
            :searchTerm="searchTerm"
            :statusFilter="statusFilter"
            />
        </div>

    </div>

</HospitalLayout>

</template>