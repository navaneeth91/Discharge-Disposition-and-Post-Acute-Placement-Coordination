import { createRouter, createWebHistory } from 'vue-router'

// Auth Views
import LoginView from '@/views/auth/LoginView.vue'
import SignupView from '@/views/auth/SignupView.vue'
import InsuranceLoginView from '@/views/auth/InsuranceLoginView.vue'

// Hospital Views
import DashboardView from '@/views/hospital/DashboardView.vue'
import CareManagerDashboardView from '@/views/careManager/DashboardView.vue'

// Layouts
import CareManagerLayout from '@/layouts/CareManagerLayout.vue'

// Insurance Views
import InsuranceDashboardView from '@/views/insurance/InsuranceDashboardView.vue'
import PhysicianDashboard from '@/views/hospital/PhysicianDashboard.vue'
import AssignedPatientsTable from '@/components/physician/AssignedPatientsTable.vue'
import ViewPatientsTable from '@/components/physician/ViewPatientsTable.vue'

import AuthorizationRequestsView from '@/views/insurance/AuthorizationRequestsView.vue'
import MemberSearchView from '@/views/insurance/MemberSearchView.vue'
import ProvidersView from '@/views/insurance/ProvidersView.vue'
import PlansView from '@/views/insurance/PlansView.vue'

// Patient Views
import PatientsView from '@/views/hospital/PatientsView.vue'
//Care Manager Views
import CareManagerPatientsView from '@/views/careManager/PatientsView.vue'
import ReferralTrackingView from '@/views/careManager/ReferralTrackingView.vue'


//Users View
import UsersView from '@/views/admin/UsersView.vue'
//Referral View
import ReferralsView from '@/views/referral/ReferralsView.vue'

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
        component: PhysicianDashboard,
        meta: {
            requiresAuth: true,
            role: 'Physician'
        }
    },
    {
        path: '/physician/AssignedPatientsTable',
        name: 'AssignedPatientsTable',
        component: AssignedPatientsTable,
        meta: {
            requiresAuth: true,
            role: 'Physician'
        }
    },
    {
        path: '/physician/ViewPatientsTable',
        name: 'ViewPatientsTable',
        component: ViewPatientsTable,
        meta: {
            requiresAuth: true,
            role: 'Physician'
        }
    },
    

    // Care Manager

   {
    path: '/care-manager',
    component: CareManagerLayout,
    meta: {
        requiresAuth: true,
        role: 'Care Manager'
    },
    children: [

        {
            path: 'dashboard',
            name: 'CareManagerDashboard',
            component: CareManagerDashboardView
        },

        {
            path: 'patients',
            name: 'CareManagerPatients',
            component: CareManagerPatientsView
        },

        {
            path: 'referrals',
            name: 'ReferralTracking',
            component: ReferralTrackingView
        }

    ]
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
    // Patients view
    {
        path: '/insurance/authorizations',
        name: 'InsuranceAuthorizations',
        component: AuthorizationRequestsView,
        meta: {
            requiresAuth: true,
            role: 'Authorization Coordinator'
        }
    },
    {
        path: '/insurance/members',
        name: 'InsuranceMembers',
        component: MemberSearchView,
        meta: {
            requiresAuth: true,
            role: 'Authorization Coordinator'
        }
    },
    {
        path: '/insurance/providers',
        name: 'InsuranceProviders',
        component: ProvidersView,
        meta: {
            requiresAuth: true,
            role: 'Authorization Coordinator'
        }
    },
    {
        path: '/insurance/plans',
        name: 'InsurancePlans',
        component: PlansView,
        meta: {
            requiresAuth: true,
            role: 'Authorization Coordinator'
        }
    },
   
    // Patients

    {
        path: '/patients',
        component: PatientsView,
        meta: {
            requiresAuth: true
        }
    },

    // Users View
    {
        path: '/administrator/users',

        component:
            UsersView,

        meta: {
            requiresAuth: true,
            role: 'Administrator'
        }
    },
    //Referral View
    {
        path: '/referrals',

        component:
            ReferralsView,

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