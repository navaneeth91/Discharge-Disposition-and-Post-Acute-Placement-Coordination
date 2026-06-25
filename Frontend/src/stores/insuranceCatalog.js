import { defineStore } from 'pinia'

import * as insuranceService from '@/services/insuranceService'

export const useInsuranceCatalogStore = defineStore('insuranceCatalog', {
    state: () => ({
        providers: [],
        plans: [],
        loading: false,
        error: ''
    }),

    actions: {
        async loadProviders() {
            this.loading = true
            this.error = ''

            try {
                const response = await insuranceService.getProviders()
                this.providers = response.data.data ?? []
            }
            catch (error) {
                this.error = error.response?.data?.message ?? 'Failed to load providers.'
                this.providers = []
            }
            finally {
                this.loading = false
            }
        },

        async loadPlans(providerId = null) {
            this.loading = true
            this.error = ''

            try {
                const response = await insuranceService.getPlans(providerId)
                this.plans = response.data.data ?? []
            }
            catch (error) {
                this.error = error.response?.data?.message ?? 'Failed to load plans.'
                this.plans = []
            }
            finally {
                this.loading = false
            }
        }
    }
})