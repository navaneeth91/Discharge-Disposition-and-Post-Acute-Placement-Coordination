<script setup>
defineProps({
    member: Object,
    show: Boolean,
    loading: Boolean
})

defineEmits(['close'])
</script>

<template>
    <Transition name="slide">
        <div v-if="show" class="fixed inset-0 z-50 flex justify-end bg-black/30">
            <div class="h-full w-full max-w-xl overflow-y-auto bg-white p-8 shadow-2xl">
                <div class="mb-8 flex items-start justify-between gap-4">
                    <div>
                        <h2 class="text-3xl font-bold text-[var(--text-primary)]">Member Details</h2>
                        <p class="text-sm text-[var(--text-secondary)]">Insurance eligibility and coverage summary.</p>
                    </div>
                    <button @click="$emit('close')" class="text-2xl text-[var(--text-secondary)]">✕</button>
                </div>

                <div v-if="loading" class="space-y-3">
                    <div class="skeleton h-24 rounded-2xl"></div>
                    <div class="skeleton h-24 rounded-2xl"></div>
                    <div class="skeleton h-24 rounded-2xl"></div>
                </div>

                <div v-else-if="member" class="space-y-6">
                    <section class="rounded-2xl border border-[var(--border)] p-4">
                        <p class="text-sm text-[var(--text-secondary)]">Name</p>
                        <h3 class="text-2xl font-semibold text-[var(--text-primary)]">{{ member.firstName }} {{ member.lastName }}</h3>
                        <p class="mt-1 text-sm text-[var(--text-secondary)]">Policy: {{ member.policyNumber }}</p>
                    </section>

                    <section class="grid gap-4 md:grid-cols-2">
                        <div class="rounded-2xl border border-[var(--border)] p-4">
                            <p class="text-sm text-[var(--text-secondary)]">DOB</p>
                            <p class="font-medium">{{ member.dob ?? member.DOB }}</p>
                        </div>
                        <div class="rounded-2xl border border-[var(--border)] p-4">
                            <p class="text-sm text-[var(--text-secondary)]">Gender</p>
                            <p class="font-medium">{{ member.gender ?? member.Gender }}</p>
                        </div>
                        <div class="rounded-2xl border border-[var(--border)] p-4">
                            <p class="text-sm text-[var(--text-secondary)]">Email</p>
                            <p class="font-medium">{{ member.email }}</p>
                        </div>
                        <div class="rounded-2xl border border-[var(--border)] p-4">
                            <p class="text-sm text-[var(--text-secondary)]">Phone</p>
                            <p class="font-medium">{{ member.phone }}</p>
                        </div>
                    </section>

                    <section>
                        <h3 class="mb-3 text-lg font-semibold text-[var(--text-primary)]">Coverages</h3>
                        <div v-if="member.coverages?.length" class="space-y-3">
                            <article v-for="coverage in member.coverages" :key="coverage.coverageId" class="rounded-2xl border border-[var(--border)] p-4">
                                <p class="font-semibold">{{ coverage.planName }}</p>
                                <p class="text-sm text-[var(--text-secondary)]">{{ coverage.planType }} · {{ coverage.insuranceProvider }}</p>
                            </article>
                        </div>
                        <EmptyState v-else />
                    </section>
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