import { defineStore } from 'pinia'

import * as authorizationService from '@/services/insuranceAuthorizationService'

export const useInsuranceAuthorizationStore = defineStore('insuranceAuthorization', {
    state: () => ({
        authorizations: [],
        recentAuthorizations: [],
        selectedAuthorization: null,
        pagination: {
            page: 1,
            pageSize: 10,
            totalCount: 0,
            totalPages: 0
        },
        loading: false,
        error: ''
    }),
    actions: {
        async loadAuthorizations({ search = '', status = '', page = 1, pageSize = 10 } = {}) {
            this.loading = true
            this.error = ''

            try {
                const response = await authorizationService.getAuthorizations({
                    search,
                    status,
                    page,
                    pageSize
                })

                const data = response.data.data

                this.authorizations = data.items
                this.pagination = {
                    page: data.page,
                    pageSize: data.pageSize,
                    totalCount: data.totalCount,
                    totalPages: data.totalPages
                }
            }
            catch (error) {
                this.error = error.response?.data?.message ?? 'Failed to load authorizations.'
                this.authorizations = []
            }
            finally {
                this.loading = false
            }
        },

        async loadRecentAuthorizations(take = 5) {
            this.loading = true
            this.error = ''

            try {
                const response = await authorizationService.getRecentAuthorizations(take)
                this.recentAuthorizations = response.data.data ?? []
            }
            catch (error) {
                this.error = error.response?.data?.message ?? 'Failed to load recent authorizations.'
                this.recentAuthorizations = []
            }
            finally {
                this.loading = false
            }
        },

        async loadAuthorization(authorizationRequestId) {
            this.loading = true
            this.error = ''

            try {
                const response = await authorizationService.getAuthorizationById(authorizationRequestId)
                this.selectedAuthorization = response.data.data
            }
            catch (error) {
                this.error = error.response?.data?.message ?? 'Failed to load authorization.'
                this.selectedAuthorization = null
            }
            finally {
                this.loading = false
            }
        }
    }
})