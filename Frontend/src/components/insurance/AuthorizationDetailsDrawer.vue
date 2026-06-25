<script setup>
import EmptyState from '@/components/common/EmptyState.vue'

defineProps({
    authorization: Object,
    show: Boolean,
    loading: Boolean
})

defineEmits([
    'close',
    'approve',
    'deny'
])

const statusClass = (status) => {
    const normalized = String(status ?? '').toLowerCase()

    if (normalized === 'approved') return 'status-approved'
    if (normalized === 'denied') return 'status-denied'

    return 'status-pending'
}
</script>

<template>
    <Transition name="slide">
        <div
            v-if="show"
            class="fixed inset-0 z-50 flex justify-end bg-black/30">

            <div
                class="h-full w-full max-w-xl overflow-y-auto bg-white p-8 shadow-2xl">

                <div
                    class="mb-8 flex items-start justify-between gap-4">

                    <div>
                        <h2
                            class="text-3xl font-bold text-[var(--text-primary)]">
                            Authorization Details
                        </h2>

                        <p
                            class="text-sm text-[var(--text-secondary)]">
                            Review authorization request details.
                        </p>
                    </div>

                    <button
                        @click="$emit('close')"
                        class="text-2xl text-[var(--text-secondary)]">
                        ✕
                    </button>
                </div>

                <div
                    v-if="loading"
                    class="space-y-3">

                    <div
                        class="skeleton h-24 rounded-2xl">
                    </div>

                    <div
                        class="skeleton h-24 rounded-2xl">
                    </div>

                    <div
                        class="skeleton h-24 rounded-2xl">
                    </div>

                </div>

                <div
                    v-else-if="authorization"
                    class="space-y-6">

                    <section
                        class="rounded-2xl border border-[var(--border)] p-4">

                        <p
                            class="text-sm text-[var(--text-secondary)]">
                            Member
                        </p>

                        <h3
                            class="text-2xl font-semibold text-[var(--text-primary)]">
                            {{ authorization.memberName }}
                        </h3>

                        <p
                            class="mt-1 text-sm text-[var(--text-secondary)]">
                            Policy:
                            {{ authorization.policyNumber }}
                        </p>

                    </section>

                    <section
                        class="grid gap-4">

                        <div
                            class="rounded-2xl border border-[var(--border)] p-4">

                            <p
                                class="text-sm text-[var(--text-secondary)]">
                                Organization
                            </p>

                            <p class="font-medium">
                                {{ authorization.requestingOrganization }}
                            </p>

                        </div>

                        <div
                            class="rounded-2xl border border-[var(--border)] p-4">

                            <p
                                class="text-sm text-[var(--text-secondary)]">
                                Service Type
                            </p>

                            <p class="font-medium">
                                {{ authorization.serviceType }}
                            </p>

                        </div>

                        <div
                            class="rounded-2xl border border-[var(--border)] p-4">

                            <p
                                class="text-sm text-[var(--text-secondary)]">
                                Requested Date
                            </p>

                            <p class="font-medium">
                                {{ new Date(authorization.requestedDate).toLocaleString() }}
                            </p>

                        </div>

                        <div
                            class="rounded-2xl border border-[var(--border)] p-4">

                            <p
                                class="text-sm text-[var(--text-secondary)]">
                                Status
                            </p>

                            <span>
                                {{ authorization.status }}
                            </span>

                        </div>

                    </section>

                    <div
                        v-if="authorization.status?.toLowerCase() === 'pending'"
                        class="mt-6 flex gap-3">

                        <button
                            class="rounded-xl bg-green-500 px-5 py-2 font-semibold text-white hover:bg-green-600"
                            @click="$emit('approve', authorization)">

                            Approve

                        </button>

                        <button
                            class="rounded-xl bg-red-500 px-5 py-2 font-semibold text-white hover:bg-red-600"
                            @click="$emit('deny', authorization)">

                            Deny

                        </button>

                    </div>

                </div>

                <div v-else>
                    <EmptyState />
                </div>

            </div>
        </div>
    </Transition>
</template>

<style scoped>
.slide-enter-active,
.slide-leave-active {
    transition: all 0.3s ease;
}

.slide-enter-from,
.slide-leave-to {
    transform: translateX(100%);
}
</style>