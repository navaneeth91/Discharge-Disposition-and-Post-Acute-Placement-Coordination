<script setup>
import EmptyState from '@/components/common/EmptyState.vue'

defineProps({
    items: {
        type: Array,
        default: () => []
    },
    loading: Boolean,
    pagination: {
        type: Object,
        default: () => ({
            page: 1,
            pageSize: 10,
            totalCount: 0,
            totalPages: 0
        })
    }
})

defineEmits(['page-change', 'select'])

const statusClass = (status) => {
    const normalized = String(status ?? '').toLowerCase()

    if (normalized === 'approved') return 'status-approved'
    if (normalized === 'denied') return 'status-denied'
    return 'status-pending'
}
</script>

<template>
    <section class="card-surface rounded-3xl p-6">
        <div class="flex items-center justify-between gap-4 mb-6">
            <div>
                <h2 class="text-xl font-semibold text-[var(--text-primary)]">Authorization Requests</h2>
                <p class="text-sm text-[var(--text-secondary)]">Search and filter requests with pagination.</p>
            </div>
        </div>

        <div v-if="loading" class="space-y-3">
            <div v-for="index in 5" :key="index" class="skeleton h-16 rounded-2xl"></div>
        </div>

        <div v-else-if="!items.length">
            <EmptyState />
        </div>

        <div v-else class="overflow-x-auto rounded-2xl border border-[var(--border)]">
            <table class="w-full min-w-[900px]  ">
                <thead>
                    <tr class="table-header">
                        <th class="px-4 py-4 text-left">Member</th>
                        <th class="px-4 py-4 text-left">Policy</th>
                        <th class="px-4 py-4 text-left">Organization</th>
                        <th class="px-4 py-4 text-left">Service</th>
                        <th class="px-4 py-4 text-left">Requested</th>
                        <th class="px-4 py-4 text-left">Status</th>
                        <th class="px-4 py-4 text-right">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in items" :key="item.authorizationRequestId" class="table-row border-b">
                        <td class="px-4 py-4 font-medium">{{ item.memberName }}</td>
                        <td class="px-4 py-4">{{ item.policyNumber }}</td>
                        <td class="px-4 py-4">{{ item.requestingOrganization }}</td>
                        <td class="px-4 py-4">{{ item.serviceType }}</td>
                        <td class="px-4 py-4">{{ new Date(item.requestedDate).toLocaleString() }}</td>
                        <td class="px-4 py-4">
                            <span class="table-badge" :class="statusClass(item.status)">{{ item.status }}</span>
                        </td>
                        <td class="px-4 py-4 text-right">
                            <button
                                    class="
                                        rounded-lg
                                        bg-[var(--primary)]
                                        px-4
                                        py-2
                                        text-sm
                                        font-semibold
                                        text-white
                                        transition
                                        hover:bg-[var(--primary-hover)]
                                    "
                                    @click="$emit('select', item)">
                                    View
                                </button>
                            </td>
                    </tr>
                </tbody>
            </table>

            <div class="mt-6 flex flex-col gap-3 border-t border-[var(--border)] pt-4 lg:flex-row lg:items-center lg:justify-between">
                <p class="text-sm text-[var(--text-secondary)]">
                    Showing {{ items.length }} of {{ pagination.totalCount }} requests
                </p>
                <div class="flex items-center gap-2">
                    <button
                        class="rounded-xl border border-[var(--border)] px-4 py-2 text-sm font-semibold disabled:opacity-50"
                        :disabled="pagination.page <= 1"
                        @click="$emit('page-change', pagination.page - 1)">
                        Previous
                    </button>
                    <span class="text-sm text-[var(--text-secondary)]">Page {{ pagination.page }} of {{ pagination.totalPages }}</span>
                    <button
                        class="rounded-xl border border-[var(--border)] px-4 py-2 text-sm font-semibold disabled:opacity-50"
                        :disabled="pagination.page >= pagination.totalPages"
                        @click="$emit('page-change', pagination.page + 1)">
                        Next
                    </button>
                </div>
            </div>
        </div>
    </section>
</template>