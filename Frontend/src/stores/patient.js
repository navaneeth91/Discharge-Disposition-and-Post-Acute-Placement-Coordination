import { defineStore } from 'pinia'
import * as patientService from '@/services/patientService'

export const usePatientStore =
defineStore('patients', {

    state: () => ({
        patients: [],
        selectedPatient: null,
        loading: false
    }),

    actions: {

        async loadPatients() {

            try {

                this.loading = true

                const response =
                    await patientService
                        .getPatients()

                this.patients =
                    response.data.data
            }
            finally {

                this.loading = false
            }
        },

        async loadPatient(id) {

            const response =
                await patientService
                    .getPatientById(id)

            this.selectedPatient =
                response.data.data
        },

        async discharge(id,date) {

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