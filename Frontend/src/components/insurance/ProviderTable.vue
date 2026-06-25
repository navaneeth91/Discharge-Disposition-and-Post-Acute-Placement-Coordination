<script setup>
import EmptyState from '@/components/common/EmptyState.vue'

defineProps({
    items: {
        type: Array,
        default: () => []
    },
    loading: Boolean
})
</script>

<template>
    <section class="card-surface rounded-3xl p-6">
        <h2 class="mb-6 text-xl font-semibold text-[var(--text-primary)]">Providers</h2>

        <div v-if="loading" class="space-y-3">
            <div v-for="index in 4" :key="index" class="skeleton h-16 rounded-2xl"></div>
        </div>

        <div v-else-if="!items.length">
            <EmptyState />
        </div>

        <div v-else class="overflow-x-auto rounded-2xl border border-[var(--border)]    ">
            <table class="w-full min-w-[720px]">
                <thead>
                    <tr class="table-header">
                        <th class="px-4 py-4 text-left">Name</th>
                        <th class="px-4 py-4 text-left">Code</th>
                        <th class="px-4 py-4 text-left">Phone</th>
                        <th class="px-4 py-4 text-left">Email</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="provider in items" :key="provider.insuranceProviderId" class="table-row border-b">
                        <td class="px-4 py-4 font-medium">{{ provider.providerName }}</td>
                        <td class="px-4 py-4">{{ provider.providerCode }}</td>
                        <td class="px-4 py-4">{{ provider.phone ?? '-' }}</td>
                        <td class="px-4 py-4">{{ provider.email ?? '-' }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </section>
</template>