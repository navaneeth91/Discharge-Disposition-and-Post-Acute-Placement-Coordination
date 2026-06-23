<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'

import AppInput from '@/components/common/AppInput.vue'
import AppButton from '@/components/common/AppButton.vue'

import { useAuthStore } from '@/stores/auth'
import * as authService from '@/services/authService'

const router = useRouter()
const authStore = useAuthStore()

const loading = ref(false)
const error = ref('')

const form = reactive({
    email: '',
    password: ''
})

const submit = async () => {

    error.value = ''

    try {

        loading.value = true

        const response =
            await authService.login(form)

        const user =
            response.data.data

        authStore.login(user)

        switch (user.role) {

            case 'Administrator':
                router.push('/administrator/dashboard')
                break

            case 'Physician':
                router.push('/physician/dashboard')
                break

            case 'Care Manager':
                router.push('/care-manager/dashboard')
                break

            case 'Post-Acute Provider':
                router.push('/provider/dashboard')
                break

            case 'Authorization Coordinator':
                router.push('/insurance/dashboard')
                break

            default:
                error.value =
                    'Account awaiting administrator approval.'
        }

    }
    catch {

        error.value =
            'Invalid email or password.'

    }
    finally {

        loading.value = false

    }
}
</script>

<template>

    <div
        class="
        fade-up
        w-full
        max-w-md
        rounded-3xl
        bg-white
        p-10
        shadow-2xl
        border
        border-slate-200">

        <h2
            class="text-3xl font-bold text-[#2D1E3E] mb-2">

            Welcome Back

        </h2>

        <p class="text-[#6F5D82] mb-8">
            Sign in to continue.
        </p>

        <div
            v-if="error"
            class="
            bg-red-100
            text-red-600
            p-3
            rounded-xl
            mb-5">

            {{ error }}

        </div>

        <form
            class="space-y-5"
            @submit.prevent="submit">

            <AppInput
                v-model="form.email"
                label="Email"
                type="email"
                placeholder="Enter email" />

            <AppInput
                v-model="form.password"
                label="Password"
                type="password"
                placeholder="Enter password" />

            <AppButton :loading="loading">
                Login
            </AppButton>

        </form>

        <div
            class="mt-8 text-center space-y-2">

            <p class="text-[#6F5D82]">
                Don't have an account?
            </p>

            <RouterLink
                to="/signup"
                class="
                font-medium
                text-[#614083]
                hover:text-[#53366F]">

                Create Account

            </RouterLink>

        </div>

    </div>

</template>