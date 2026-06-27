import { defineStore } from 'pinia'

import {
    getDashboard,
    getMyPatients,
    getReferralTracking,
    getAuthorizationTracking
} from '@/services/careManagerService'

export const useCareManagerStore = defineStore(
    'careManager',
    {
        state: () => ({

            loading: false,

            error: null,

            dashboard: null,

            // ---------------- Patients ----------------

            assignedPatients: [],

            page: 1,

            pageSize: 10,

            totalPages: 1,

            totalRecords: 0,

            search: '',

            // ---------------- Referrals ----------------

            referrals: [],

            referralPage: 1,

            referralPageSize: 10,

            referralTotalPages: 1,

            referralTotalRecords: 0,

            referralSearch: '',

            referralStatus: null,
                        // ---------------- Authorization Tracking ----------------

            authorizationTracking: [],

            authorizationPage: 1,

            authorizationPageSize: 10,

            authorizationTotalPages: 1,

            authorizationTotalRecords: 0,

            authorizationSearch: '',

            authorizationStatus: null,

        }),

        actions: {

            // ================= Dashboard =================

            async loadDashboard(careManagerId) {

                try {

                    this.loading = true

                    this.error = null

                    const response =
                        await getDashboard(careManagerId)

                    this.dashboard =
                        response.data.data

                    await this.loadPatients(careManagerId)

                }

                catch (err) {

                    this.error =
                        err.response?.data?.message ||
                        'Failed to load dashboard.'

                }

                finally {

                    this.loading = false

                }

            },

            // ================= Patients =================

            async loadPatients(careManagerId) {

                try {

                    this.loading = true

                    this.error = null

                    const response =
                        await getMyPatients(
                            careManagerId,
                            this.page,
                            this.pageSize,
                            this.search
                        )

                    const data =
                        response.data.data

                    this.assignedPatients =
                        data.items

                    this.page =
                        data.page

                    this.pageSize =
                        data.pageSize

                    this.totalPages =
                        data.totalPages

                    this.totalRecords =
                        data.totalRecords

                }

                catch (err) {

                    this.error =
                        err.response?.data?.message ||
                        'Failed to load patients.'

                }

                finally {

                    this.loading = false

                }

            },

            async nextPage(careManagerId) {

                if (
                    this.page >=
                    this.totalPages
                )
                    return

                this.page++

                await this.loadPatients(careManagerId)

            },

            async previousPage(careManagerId) {

                if (this.page <= 1)
                    return

                this.page--

                await this.loadPatients(careManagerId)

            },

            async searchPatients(
                careManagerId,
                search
            ) {

                this.search = search

                this.page = 1

                await this.loadPatients(careManagerId)

            },

            // ================= Referral Tracking =================

            async loadReferralTracking(careManagerId) {

                try {

                    this.loading = true

                    this.error = null

                    const response =
                        await getReferralTracking(

                            careManagerId,

                            this.referralPage,

                            this.referralPageSize,

                            this.referralSearch,

                            this.referralStatus

                        )

                    const data =
                        response.data.data

                    this.referrals =
                        data.items

                    this.referralPage =
                        data.page

                    this.referralPageSize =
                        data.pageSize

                    this.referralTotalPages =
                        data.totalPages

                    this.referralTotalRecords =
                        data.totalRecords

                }

                catch (err) {

                    this.error =
                        err.response?.data?.message ||
                        'Failed to load referrals.'

                }

                finally {

                    this.loading = false

                }

            },

            async nextReferralPage(careManagerId) {

                if (
                    this.referralPage >=
                    this.referralTotalPages
                )
                    return

                this.referralPage++

                await this.loadReferralTracking(
                    careManagerId
                )

            },

            async previousReferralPage(careManagerId) {

                if (
                    this.referralPage <= 1
                )
                    return

                this.referralPage--

                await this.loadReferralTracking(
                    careManagerId
                )

            },

            async searchReferrals(
                careManagerId,
                search
            ) {

                this.referralSearch =
                    search

                this.referralPage = 1

                await this.loadReferralTracking(
                    careManagerId
                )

            },

            async filterReferrals(
                careManagerId,
                status
            ) {

                this.referralStatus =
                    status

                this.referralPage = 1

                await this.loadReferralTracking(
                    careManagerId
                )

            },
            // ================= Authorization Tracking =================

async loadAuthorizationTracking(careManagerId) {

    try {

        this.loading = true

        this.error = null

        const response =
            await getAuthorizationTracking(

                careManagerId,

                this.authorizationPage,

                this.authorizationPageSize,

                this.authorizationSearch,

                this.authorizationStatus

            )

        const data =
            response.data.data

        this.authorizationTracking =
            data.items

        this.authorizationPage =
            data.page

        this.authorizationPageSize =
            data.pageSize

        this.authorizationTotalPages =
            data.totalPages

        this.authorizationTotalRecords =
            data.totalRecords

    }

    catch (err) {

        this.error =
            err.response?.data?.message ||
            'Failed to load authorization tracking.'

    }

    finally {

        this.loading = false

    }

},

async searchAuthorizationTracking(
    careManagerId,
    search
) {

    this.authorizationSearch =
        search

    this.authorizationPage = 1

    await this.loadAuthorizationTracking(
        careManagerId
    )

},

async filterAuthorizationTracking(
    careManagerId,
    status
) {

    this.authorizationStatus =
        status

    this.authorizationPage = 1

    await this.loadAuthorizationTracking(
        careManagerId
    )

},

async nextAuthorizationPage(
    careManagerId
) {

    if (
        this.authorizationPage >=
        this.authorizationTotalPages
    )
        return

    this.authorizationPage++

    await this.loadAuthorizationTracking(
        careManagerId
    )

},

async previousAuthorizationPage(
    careManagerId
) {

    if (
        this.authorizationPage <= 1
    )
        return

    this.authorizationPage--

    await this.loadAuthorizationTracking(
        careManagerId
    )

}
            

        }

    }
)
