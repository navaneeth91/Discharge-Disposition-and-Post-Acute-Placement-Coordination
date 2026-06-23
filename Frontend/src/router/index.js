import { createRouter, createWebHistory } from 'vue-router'

import LoginView from '@/views/LoginView.vue'
import SignupView from '@/views/SignupView.vue'
import InsuranceLoginView from '@/views/InsuranceLoginView.vue'

const routes = [
  {
    path: '/',
    redirect: '/login'
  },
  {
    path: '/login',
    name: 'login',
    component: LoginView
  },
  {
    path: '/signup',
    name: 'signup',
    component: SignupView
  },
  {
    path: '/insurance/login',
    name: 'insuranceLogin',
    component: InsuranceLoginView
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router