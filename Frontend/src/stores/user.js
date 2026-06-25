import { defineStore }
from 'pinia'

import * as userService
from '@/services/userService'

export const useUserStore =
defineStore('users', {

    state: () => ({
        users: [],
        selectedUser: null,
        loading: false
    }),

    actions: {

        async loadUsers() {

            try {

                this.loading = true

                const response =
                    await userService
                        .getUsers()

                this.users =
                    response.data.data
            }
            finally {

                this.loading = false
            }
        },

        async loadUser(id) {

            const response =
                await userService
                    .getUser(id)

            this.selectedUser =
                response.data.data
        },

        async updateUser(id,data) {

            await userService
                .updateUser(
                    id,
                    data
                )

            await this.loadUsers()
        }
    }
})