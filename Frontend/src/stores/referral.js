import { defineStore }
from 'pinia'

import * as referralService
from '@/services/referralService'

export const useReferralStore =
defineStore('referrals', {

    state: () => ({

        referrals: [],

        selectedReferral: null,

        loading: false,

        page: 1,

        pageSize: 10,

        totalPages: 1,

        totalCount: 0,

        search: '',

        status: 'all'
    }),

    actions: {

        async loadReferrals() {

            try {

                this.loading = true

                const response =
                    await referralService
                        .getReferrals(
                            this.page,
                            this.pageSize,
                            this.search,
                            this.status
                        )

                const data =
                    response.data.data

                this.referrals =
                    data.items

                this.totalPages =
                    data.totalPages

                this.totalCount =
                    data.totalCount
            }
            finally {

                this.loading = false
            }
        },

        async loadReferral(id) {

            const response =
                await referralService
                    .getReferral(id)

            this.selectedReferral =
                response.data.data
        },

        async updateStatus(
            id,
            status
        ) {

            await referralService
                .updateStatus(
                    id,
                    status
                )

            await this.loadReferrals()
        },

        async searchReferrals(value) {

            this.search = value

            this.page = 1

            await this.loadReferrals()
        },

        async setStatus(status) {

            this.status = status

            this.page = 1

            await this.loadReferrals()
        },

        async nextPage() {

            if (
                this.page <
                this.totalPages
            ) {

                this.page++

                await this.loadReferrals()
            }
        },

        async previousPage() {

            if (
                this.page > 1
            ) {

                this.page--

                await this.loadReferrals()
            }
        },

        async goToPage(page) {

            this.page = page

            await this.loadReferrals()
        }
    }
})