import { defineStore } from 'pinia'

import * as memberService from '@/services/memberService'

export const useInsuranceMemberStore = defineStore('insuranceMember', {
    state: () => ({
        searchResults: [],
        selectedMember: null,
        loading: false,
        error: ''
    }),

    actions: {
        async searchMembers(query, take = 20) {
            this.loading = true
            this.error = ''

            try {
                const response = await memberService.searchMembers(query, take)
                this.searchResults = response.data.data ?? []
            }
            catch (error) {
                this.error = error.response?.data?.message ?? 'Failed to search members.'
                this.searchResults = []
            }
            finally {
                this.loading = false
            }
        },

        async loadMember(memberId) {
            this.loading = true
            this.error = ''

            try {
                const response = await memberService.getMemberById(memberId)
                this.selectedMember = response.data.data
            }
            catch (error) {
                this.error = error.response?.data?.message ?? 'Failed to load member.'
                this.selectedMember = null
            }
            finally {
                this.loading = false
            }
        }
    }
})