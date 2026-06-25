<script setup>
import AppInput from '@/components/common/AppInput.vue'
import AppButton from '@/components/common/AppButton.vue'

defineProps({
    modelValue: String,
    loading: Boolean
})

defineEmits(['update:modelValue', 'search'])
</script>

<template>
    <section
        class="
            card-surface
            rounded-3xl
            border
            border-[var(--border)]
            p-6
            shadow-sm
        ">
        <div class="mb-6">
            <h2 class="text-xl font-semibold text-[var(--text-primary)]">
                Member Search
            </h2>
            <p class="text-sm text-[var(--text-secondary)]">
                Search members by name, policy number, email, or phone.
            </p>
        </div>

        <div class="grid gap-4 lg:grid-cols-[1fr_auto] lg:items-end">
            <AppInput
                :model-value="modelValue"
                label="Search Member"
                placeholder="Enter name, policy number, email, or phone"
                @update:modelValue="$emit('update:modelValue', $event)"
            />

            <AppButton
                class="
                    lg:w-40
                    bg-[var(--primary)]
                    hover:bg-[var(--primary-hover)]
                    text-white
                "
                :loading="loading"
                @click="$emit('search')">
                Search
            </AppButton>
        </div>
    </section>
</template>