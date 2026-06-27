<script setup>
import AppInput from '@/components/common/AppInput.vue'

defineProps({
    search: String,
    status: String,
    loading: Boolean
})

defineEmits([
    'update:search',
    'update:status',
    'reset'
])

const statusOptions = [
    { label: 'All Statuses', value: '' },
    { label: 'Pending', value: 'Pending' },
    { label: 'Approved', value: 'Approved' },
    { label: 'Denied', value: 'Denied' }
]
</script>

<template>
    <section class="card-surface rounded-3xl p-6">
        <div class="grid gap-4 lg:grid-cols-3">
            <AppInput
                :model-value="search"
                label="Search"
                placeholder="Search member, policy, service, organization"
                @update:modelValue="$emit('update:search', $event)" />

            <div class="space-y-2">
                <label class="text-sm font-medium text-slate-700">Status</label>
                <select
                    :value="status"
                    @change="$emit('update:status', $event.target.value)"
                    class="w-full rounded-xl border bg-white px-4 py-3 outline-none transition-all duration-300 focus:ring-4"
                    style="border-color: var(--border);">
                    <option v-for="option in statusOptions" :key="option.value" :value="option.value">
                        {{ option.label }}
                    </option>
                </select>
            </div>

            <div class="flex items-end gap-3">
                <button
                    type="button"
                    @click="$emit('reset')"
                    class=" rounded-xl border border-[var(--border)] bg-white px-4 py-3 font-semibold text-[var(--text-primary)] transition hover:bg-slate-50">
                    Reset
                </button>
            </div>
        </div>
    </section>
</template>