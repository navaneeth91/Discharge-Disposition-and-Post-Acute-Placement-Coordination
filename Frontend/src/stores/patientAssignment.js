import { defineStore } from 'pinia'
import patientAssignmentService from '@/services/patientAssignmentService'

export const usePatientAssignmentStore = defineStore('patientAssignment', {
  state: () => ({
    patients: [],
    loading: false,
    error: null
  }),

  actions: {
    async fetchPatients(careManagerId) {
      this.loading = true
      this.error = null

      try {
        const response =
          await patientAssignmentService.getPatientsByCareManager(careManagerId)

        if (response.success) {
          this.patients = response.data
        } else {
          this.error = response.message
        }
      } catch (err) {
        this.error =
          err.response?.data?.message ||
          err.message ||
          'Failed to fetch patients.'
      } finally {
        this.loading = false
      }
    },

    clearPatients() {
      this.patients = []
    }
  }
})
