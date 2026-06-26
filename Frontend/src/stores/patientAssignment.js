import { defineStore }
from 'pinia'

import * as assignmentService
from '@/services/patientAssignmentService'

export const usePatientAssignmentStore =
defineStore(
'patientAssignment',
{

    state: () => ({

        unassignedPatients: [],

        careManagers: [],

        loading: false,

        page: 1,

        pageSize: 10,

        totalPages: 1,

        totalCount: 0,

        search: ''

    }),

    actions: {

        async loadData() {

            try {

                this.loading = true

                const [

                    patients,

                    managers

                ] = await Promise.all([

                    assignmentService
                        .getUnassignedPatients(

                            this.page,

                            this.pageSize,

                            this.search

                        ),

                    assignmentService
                        .getCareManagers()

                ])

                const patientData =
                    patients.data.data

                this.unassignedPatients =
                    patientData.items

                this.totalPages =
                    patientData.totalPages

                this.totalCount =
                    patientData.totalCount

                this.careManagers =
                    managers.data.data ?? []

            }

            catch (error) {

                console.error(
                    'Failed to load assignment data',
                    error
                )

                this.unassignedPatients = []

                this.careManagers = []

                this.totalPages = 1

                this.totalCount = 0

            }

            finally {

                this.loading = false

            }

        },

        async assignPatient(
            patientId,
            careManagerId,
            assignedBy,
            notes = ''
        ) {

            try {

                await assignmentService
                    .assignCareManager({

                        patientId,

                        careManagerId,

                        assignedBy,

                        notes

                    })

                const index =
                    this.unassignedPatients
                        .findIndex(

                            x =>

                                x.patientId ===
                                patientId

                        )

                if (index !== -1) {

                    this.unassignedPatients
                        .splice(index, 1)

                }

                this.totalCount--

                return true

            }

            catch (error) {

                console.error(
                    'Assignment failed',
                    error
                )

                throw error

            }

        },

        async searchPatients(value) {

            this.search = value

            this.page = 1

            await this.loadData()

        },

        async goToPage(page) {

            this.page = page

            await this.loadData()

        },

        async nextPage() {

            if (
                this.page <
                this.totalPages
            ) {

                this.page++

                await this.loadData()

            }

        },

        async previousPage() {

            if (
                this.page > 1
            ) {

                this.page--

                await this.loadData()

            }

        },

        async refresh() {

            await this.loadData()

        }

    }

})