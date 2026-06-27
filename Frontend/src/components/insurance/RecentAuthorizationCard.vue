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
        <div class="flex items-center justify-between gap-4 mb-6">
            <div>
                <h2 class="text-xl font-semibold text-[var(--text-primary)]">Recent Authorization Requests</h2>
                <p class="text-sm text-[var(--text-secondary)]">Latest requests submitted to the insurance team.</p>
            </div>
        </div>

        <div v-if="loading" class="space-y-3">
            <div v-for="index in 4" :key="index" class="skeleton h-20 rounded-2xl"></div>
        </div>

        <div v-else-if="!items.length">
            <EmptyState />
        </div>

        <div v-else class="space-y-3">
            <article
                v-for="item in items"
                :key="item.authorizationRequestId"
                class="rounded-2xl border border-[var(--border)] p-4 transition hover:-translate-y-0.5 hover:shadow-lg">
                <div class="flex flex-col gap-3 lg:flex-row lg:items-center lg:justify-between">
                    <div>
                        <p class="font-semibold text-[var(--text-primary)]">{{ item.memberName }}</p>
                        <p class="text-sm text-[var(--text-secondary)]">{{ item.policyNumber }} · {{ item.requestingOrganization }}</p>
                        <p class="text-sm text-[var(--text-secondary)]">{{ item.serviceType }}</p>
                    </div>

                    <div class="flex flex-wrap items-center gap-3">
                        <span class="table-badge status-pending" :class="statusClass(item.status)">
                            {{ item.status }}
                        </span>
                        <span class="text-sm text-[var(--text-secondary)]">{{ formatDate(item.requestedDate) }}</span>
                    </div>
                </div>
            </article>
        </div>
    </section>
</template>

<script>
export default {
    methods: {
        formatDate(value) {
            return new Date(value).toLocaleString()
        },
        statusClass(status) {
            const normalized = String(status ?? '').toLowerCase()

            if (normalized === 'approved') return 'status-approved'
            if (normalized === 'denied') return 'status-denied'
            return 'status-pending'
        }
    }
}
</script>