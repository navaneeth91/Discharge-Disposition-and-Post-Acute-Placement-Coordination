<script setup>
import { computed, onMounted, ref } from 'vue'

import InsuranceLayout from '@/layouts/InsuranceLayout.vue'
import PlanTable from '@/components/insurance/PlanTable.vue'
import { useInsuranceCatalogStore } from '@/stores/insuranceCatalog'

const store = useInsuranceCatalogStore()
const selectedProviderId = ref('')

const providerName = computed(() => {
    const provider = store.providers.find(item => String(item.insuranceProviderId) === String(selectedProviderId.value))
    return provider?.providerName ?? ''
})

const loadPlans = async () => {
    await store.loadPlans(selectedProviderId.value || null)
}

onMounted(async () => {
    await store.loadProviders()
    await loadPlans()
})
</script>

<template>
    <InsuranceLayout>
        <div class="space-y-6 fade-up">
            <div>
                <h1 class="text-3xl font-bold text-[var(--text-primary)]">Plans</h1>
                <p class="mt-2 text-[var(--text-secondary)]">View all insurance plans and filter by provider.</p>
            </div>

            <section class="card-surface rounded-3xl p-6">
                <div class="grid gap-4 lg:grid-cols-[1fr_auto] lg:items-end">
                    <div class="space-y-2">
                        <label class="text-sm font-medium text-slate-700">Provider Filter</label>
                        <select
                            v-model="selectedProviderId"
                            @change="loadPlans"
                            class="w-full rounded-xl border bg-white px-4 py-3 outline-none transition-all duration-300 focus:ring-4"
                            style="border-color: var(--border);">
                            <option value="">All Providers</option>
                            <option v-for="provider in store.providers" :key="provider.insuranceProviderId" :value="provider.insuranceProviderId">
                                {{ provider.providerName }}
                            </option>
                        </select>
                    </div>
                </div>
            </section>

            <PlanTable :items="store.plans" :loading="store.loading" :provider-name="providerName" />
        </div>
    </InsuranceLayout>
</template>