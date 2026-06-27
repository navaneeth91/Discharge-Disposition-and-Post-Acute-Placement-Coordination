import { defineStore } from 'pinia'

import * as dashboardService from '@/services/dashboardService';
import * as physicianService from "@/services/physicianService";

export const useDashboardStore =
    defineStore('dashboard', {

        state: () => ({
            hospitalStats: null,
            insuranceStats: null,
            physicianStats: null,
            currentSection: "dashboard",
            loading: false,
            assignedPatients: [],
        }),

        
        actions: {
            setCurrentSection(section){
                this.currentSection = section;
            },
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

            async loadPhysicianDashboard() {

                this.loading = true;

                try {

                    const response = await dashboardService.getPhysicianDashboard();

                    this.physicianStats =
                        response.data.data;

                }

                finally {

                    this.loading = false;

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