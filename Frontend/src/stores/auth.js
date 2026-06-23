import { defineStore } from 'pinia'

export const useAuthStore = defineStore('auth', {

    state: () => ({
        token: sessionStorage.getItem('token'),
        role: sessionStorage.getItem('role'),
        userId: sessionStorage.getItem('userId'),
        firstName: sessionStorage.getItem('firstName')
    }),

    actions: {

        login(user) {

            this.token = user.token
            this.role = user.role
            this.userId = user.userId
            this.firstName = user.firstName

            sessionStorage.setItem(
                'token',
                user.token
            )

            sessionStorage.setItem(
                'role',
                user.role
            )

            sessionStorage.setItem(
                'userId',
                user.userId
            )

            sessionStorage.setItem(
                'firstName',
                user.firstName
            )
        },

        logout() {

            this.token = null
            this.role = null
            this.userId = null
            this.firstName = null

            sessionStorage.clear()
        }
    }
})