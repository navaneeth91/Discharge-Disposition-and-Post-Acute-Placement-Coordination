import { defineStore }
from 'pinia'

import * as patientService
from '@/services/patientService'

export const usePatientStore =
defineStore('patients', {

    state: () => ({

        patients: [],

        selectedPatient: null,

        loading: false,

        page: 1,

        pageSize: 10,

        totalPages: 1,

        totalCount: 0,

        search: '',

        status: 'all'
    }),

    actions: {

        async loadPatients() {

            try {

                this.loading = true

                const response =
                    await patientService
                        .getPatients(
                            this.page,
                            this.pageSize,
                            this.search,
                            this.status
                        )

                const data =
                    response.data.data

                this.patients =
                    data.items

                this.totalPages =
                    data.totalPages

                this.totalCount =
                    data.totalCount
            }
            catch (error) {

                console.error(error)
            }
            finally {

                this.loading = false
            }
        },

        async searchPatients(value) {

            this.search = value

            this.page = 1

            await this.loadPatients()
        },

        async setStatus(status) {

            this.status = status

            this.page = 1

            await this.loadPatients()
        },

        async goToPage(page) {

            this.page = page

            await this.loadPatients()
        },

        async nextPage() {

            if (
                this.page <
                this.totalPages
            ) {

                this.page++

                await this.loadPatients()
            }
        },

        async previousPage() {

            if (
                this.page > 1
            ) {

                this.page--

                await this.loadPatients()
            }
        },

        async loadPatient(id) {

            const response =
                await patientService
                    .getPatientById(id)

            this.selectedPatient =
                response.data.data
        },

        async discharge(
            id,
            date
        ) {

            await patientService
                .dischargePatient(
                    id,
                    date
                )

            await this.loadPatients()

            this.selectedPatient =
                null
        }
    }
})