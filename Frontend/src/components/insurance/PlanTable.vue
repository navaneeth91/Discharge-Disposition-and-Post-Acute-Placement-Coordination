<script setup>
import EmptyState from '@/components/common/EmptyState.vue'

defineProps({
    items: {
        type: Array,
        default: () => []
    },
    loading: Boolean,
    providerName: String
})
</script>

<template>
    <section class="card-surface rounded-3xl p-6">
        <div class="mb-6 flex flex-col gap-2 lg:flex-row lg:items-end lg:justify-between">
            <div>
                <h2 class="text-xl font-semibold text-[var(--text-primary)]">Plans</h2>
                <p class="text-sm text-[var(--text-secondary)]">{{ providerName ? `Filtered by ${providerName}` : 'All insurance plans' }}</p>
            </div>
        </div>

        <div v-if="loading" class="space-y-3">
            <div v-for="index in 4" :key="index" class="skeleton h-16 rounded-2xl"></div>
        </div>

        <div v-else-if="!items.length">
            <EmptyState />
        </div>

        <div v-else class="overflow-x-auto rounded-2xl border border-[var(--border)]">
            <table class="w-full min-w-[720px]">
                <thead>
                    <tr class="table-header">
                        <th class="px-4 py-4 text-left">Plan</th>
                        <th class="px-4 py-4 text-left">Type</th>
                        <th class="px-4 py-4 text-left">Provider</th>
                        <th class="px-4 py-4 text-left">Status</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="plan in items" :key="plan.planId" class="table-row border-b">
                        <td class="px-4 py-4 font-medium">{{ plan.planName }}</td>
                        <td class="px-4 py-4">{{ plan.planType }}</td>
                        <td class="px-4 py-4">{{ plan.providerName ?? plan.insuranceProviderName ?? '-' }}</td>
                        <td class="px-4 py-4">
                            <span class="table-badge " :class="plan.isActive ? 'status-approved' : 'status-denied'">
                                {{ plan.isActive ? 'Active' : 'Inactive' }}
                            </span>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </section>
</template>