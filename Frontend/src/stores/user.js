import { defineStore }
from 'pinia'

import * as userService
from '@/services/userService'

export const useUserStore =
defineStore('users', {

    state: () => ({

        users: [],

        selectedUser: null,

        loading: false,

        page: 1,

        pageSize: 10,

        totalPages: 1,

        totalCount: 0,

        search: ''
    }),

    actions: {

        async loadUsers() {

            try {

                this.loading = true

                const response =
                    await userService
                        .getUsers(
                            this.page,
                            this.pageSize,
                            this.search
                        )

                const data =
                    response.data.data

                this.users =
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

        async searchUsers(value) {

            this.search = value

            this.page = 1

            await this.loadUsers()
        },

        async goToPage(page) {

            this.page = page

            await this.loadUsers()
        },

        async nextPage() {

            if (
                this.page <
                this.totalPages
            ) {

                this.page++

                await this.loadUsers()
            }
        },

        async previousPage() {

            if (
                this.page > 1
            ) {

                this.page--

                await this.loadUsers()
            }
        },

        async loadUser(id) {

            const response =
                await userService
                    .getUser(id)

            this.selectedUser =
                response.data.data
        },

        async updateUser(
            id,
            data
        ) {

            console.log(id)

            console.log(data)

            await userService
                .updateUser(
                    id,
                    data
                )

            await this.loadUsers()
        }
    }
})