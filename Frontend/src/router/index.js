import { createRouter, createWebHistory } from 'vue-router'

// Auth Views
import LoginView from '@/views/auth/LoginView.vue'
import SignupView from '@/views/auth/SignupView.vue'
import InsuranceLoginView from '@/views/auth/InsuranceLoginView.vue'

// Hospital Views
import DashboardView from '@/views/hospital/DashboardView.vue'

// Insurance Views
import InsuranceDashboardView from '@/views/insurance/InsuranceDashboardView.vue'

//Patient Views
import PatientsView from '@/views/hospital/PatientsView.vue'

const routes = [

    // Default Redirect
    {
        path: '/',
        redirect: '/login'
    },

    // Authentication Routes

    {
        path: '/login',
        name: 'Login',
        component: LoginView
    },

    {
        path: '/signup',
        name: 'Signup',
        component: SignupView
    },

    {
        path: '/insurance/login',
        name: 'InsuranceLogin',
        component: InsuranceLoginView
    },

    // Administrator

    {
        path: '/administrator/dashboard',
        name: 'AdministratorDashboard',
        component: DashboardView,
        meta: {
            requiresAuth: true,
            role: 'Administrator'
        }
    },

    // Physician

    {
        path: '/physician/dashboard',
        name: 'PhysicianDashboard',
        component: DashboardView,
        meta: {
            requiresAuth: true,
            role: 'Physician'
        }
    },

    // Care Manager

    {
        path: '/care-manager/dashboard',
        name: 'CareManagerDashboard',
        component: DashboardView,
        meta: {
            requiresAuth: true,
            role: 'Care Manager'
        }
    },

    // Post Acute Provider

    {
        path: '/provider/dashboard',
        name: 'ProviderDashboard',
        component: DashboardView,
        meta: {
            requiresAuth: true,
            role: 'Post-Acute Provider'
        }
    },

    // Insurance

    {
        path: '/insurance/dashboard',
        name: 'InsuranceDashboard',
        component: InsuranceDashboardView,
        meta: {
            requiresAuth: true,
            role: 'Authorization Coordinator'
        }
    },
    {
        path: '/patients',

        component:
            PatientsView,

        meta: {
            requiresAuth: true
        }
    },
    // 404

    {
        path: '/:pathMatch(.*)*',
        redirect: '/login'
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach((to, from, next) => {

    const token =
        sessionStorage.getItem('token')

    const role =
        sessionStorage.getItem('role')

    const publicPages = [
        '/login',
        '/signup',
        '/insurance/login'
    ]

    // Public routes

    if (publicPages.includes(to.path)) {

        // Already logged in

        if (token && role) {

            switch (role) {

                case 'Administrator':
                    next('/administrator/dashboard')
                    return

                case 'Physician':
                    next('/physician/dashboard')
                    return

                case 'Care Manager':
                    next('/care-manager/dashboard')
                    return

                case 'Post-Acute Provider':
                    next('/provider/dashboard')
                    return

                case 'Authorization Coordinator':
                    next('/insurance/dashboard')
                    return
            }
        }

        next()
        return
    }

    // User not authenticated

    if (to.meta.requiresAuth && !token) {

        next('/login')
        return
    }

    // Role mismatch

    if (
        to.meta.role &&
        role !== to.meta.role
    ) {
        next('/login')
        return
    }

    next()
})

export default router