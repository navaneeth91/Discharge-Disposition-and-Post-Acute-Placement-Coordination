import { defineStore } from 'pinia'

import * as dashboardService
from '@/services/dashboardService'

export const useDashboardStore =
    defineStore('dashboard', {

        state: () => ({
            hospitalStats: null,
            insuranceStats: null,
            recentInsuranceAuthorizations: [],
            loading: false,
            recentLoading: false
        }),

        actions: {

            async loadHospitalDashboard() {

                this.loading = true

                try {

                    const response =
                        await dashboardService
                            .getHospitalDashboard()

                    this.hospitalStats =
                        response.data.data

                }
                finally {

                    this.loading = false

                }
            },

            async loadInsuranceDashboard() {

                this.loading = true

                try {

                    const response =
                        await dashboardService
                            .getInsuranceDashboard()

                    this.insuranceStats =
                        response.data.data

                }
                finally {

                    this.loading = false

                }
            },

            async loadRecentInsuranceAuthorizations() {

                this.recentLoading = true

                try {

                    const response =
                        await dashboardService
                            .getRecentInsuranceAuthorizations()

                    this.recentInsuranceAuthorizations =
                        response.data.data ?? []

                }
                catch (error) {

                    this.recentInsuranceAuthorizations = []

                    console.error(error)

                }
                finally {

                    this.recentLoading = false

                }
            }
        }
    })