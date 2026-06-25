import { defineStore } from 'pinia'

import * as dashboardService
from '@/services/dashboardService'

export const useDashboardStore =
    defineStore('dashboard', {

        state: () => ({
            hospitalStats: null,
            insuranceStats: null,
            loading: false
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
            }
        }
    })