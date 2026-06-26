<script setup>
import { onMounted } from 'vue'

import InsuranceLayout from '@/layouts/InsuranceLayout.vue'
import ProviderTable from '@/components/insurance/ProviderTable.vue'
import { useInsuranceCatalogStore } from '@/stores/insuranceCatalog'

const store = useInsuranceCatalogStore()

onMounted(async () => {
    await store.loadProviders()
})
</script>

<template>
    <InsuranceLayout>
        <div class="space-y-6 fade-up">
            <div>
                <h1 class="text-3xl font-bold text-[var(--text-primary)]">Providers</h1>
                <p class="mt-2 text-[var(--text-secondary)]">View all insurance providers.</p>
            </div>

            <ProviderTable :items="store.providers" :loading="store.loading" />
        </div>
    </InsuranceLayout>
</template>