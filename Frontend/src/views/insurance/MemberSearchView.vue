<script setup>
import { onMounted, ref, watch } from 'vue'
import { useDebounceFn } from '@vueuse/core'

import InsuranceLayout from '@/layouts/InsuranceLayout.vue'
import MemberSearch from '@/components/insurance/MemberSearch.vue'
import MemberDetailsDrawer from '@/components/insurance/MemberDetailsDrawer.vue'
import EmptyState from '@/components/common/EmptyState.vue'
import { useInsuranceMemberStore } from '@/stores/insuranceMember'

const store = useInsuranceMemberStore()

const query = ref('')
const showDrawer = ref(false)

const debouncedSearch = useDebounceFn(async () => {
    if (!query.value.trim()) {
        store.searchResults = []
        return
    }

    await store.searchMembers(query.value)
}, 350)

const search = async () => {
    if (!query.value.trim()) {
        store.searchResults = []
        return
    }

    await store.searchMembers(query.value)
}

const openMember = async (member) => {
    showDrawer.value = true
    await store.loadMember(member.memberId)
}

watch(query, () => debouncedSearch())

onMounted(() => {
    store.searchResults = []
})
</script>

<template>
    <InsuranceLayout>
        <div class="space-y-6 fade-up">
            <div>
                <h1 class="text-3xl font-bold text-[var(--text-primary)]">Member Search</h1>
                <p class="mt-2 text-[var(--text-secondary)]">Search members and inspect coverage details.</p>
            </div>

            <MemberSearch v-model="query" :loading="store.loading" @search="search" />

            <section class="card-surface rounded-3xl p-6">
                <h2 class="mb-6 text-xl font-semibold text-[var(--text-primary)]">Results</h2>

                <div v-if="store.loading" class="space-y-3">
                    <div v-for="index in 4" :key="index" class="skeleton h-16 rounded-2xl"></div>
                </div>

                <div v-else-if="!store.searchResults.length">
                    <EmptyState />
                </div>

                <div v-else class="overflow-x-auto rounded-2xl border border-[var(--border)]">
                    <table class="w-full min-w-[900px]">
                        <thead>
                            <tr class="table-header">
                                <th class="px-4 py-4 text-left">Member</th>
                                <th class="px-4 py-4 text-left">Policy</th>
                                <th class="px-4 py-4 text-left">Contact</th>
                                <th class="px-4 py-4 text-left">Coverages</th>
                                <th class="px-4 py-4 text-right">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="member in store.searchResults" :key="member.memberId" class="table-row border-b">
                                <td class="px-4 py-4 font-medium">{{ member.fullName }}</td>
                                <td class="px-4 py-4">{{ member.policyNumber }}</td>
                                <td class="px-4 py-4">{{ member.email }}<br>{{ member.phone }}</td>
                                <td class="px-4 py-4">{{ member.coverageCount }} coverages · {{ member.authorizationCount }} authorizations</td>
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
                                    @click="openMember(member)">
                                    View
                                </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </section>
        </div>

        <MemberDetailsDrawer
            :member="store.selectedMember"
            :show="showDrawer"
            :loading="store.loading"
            @close="showDrawer = false" />
    </InsuranceLayout>
</template>