<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'

import AppInput from '@/components/common/AppInput.vue'
import AppButton from '@/components/common/AppButton.vue'

import * as authService from '@/services/authService'

const router = useRouter()

const loading = ref(false)
const success = ref(false)
const error = ref('')

const form = reactive({
    userName: '',
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    password: '',
    confirmPassword: ''
})

const submit = async () => {

    error.value = ''

    if (form.password !== form.confirmPassword) {

        error.value =
            'Passwords do not match.'

        return
    }

    try {

        loading.value = true

        await authService.signup({

            userName: form.userName,
            firstName: form.firstName,
            lastName: form.lastName,
            email: form.email,
            phoneNumber: form.phoneNumber,
            password: form.password

        })

        success.value = true

    }
    catch (err) {

        error.value =
            err.response?.data?.message ??
            'Signup failed.'

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
        max-w-xl
        rounded-3xl
        bg-white
        p-10
        shadow-2xl
        border
        border-slate-200">

        <template v-if="success">

            <h2
                class="
                text-3xl
                font-bold
                text-green-600
                mb-4">

                Registration Successful

            </h2>

            <p class="text-[#6F5D82]">
                Your account has been created.
            </p>

            <p class="text-[#6F5D82] mt-2">
                Please wait for administrator approval.
            </p>

            <button
                @click="router.push('/login')"
                class="
                mt-8
                w-full
                rounded-xl
                bg-[#614083]
                py-3
                text-white
                hover:bg-[#53366F]
                transition">

                Back to Login

            </button>

        </template>

        <template v-else>

            <h2
                class="
                text-3xl
                font-bold
                text-[#2D1E3E]
                mb-2">

                Create Account

            </h2>

            <p
                class="
                text-[#6F5D82]
                mb-8">

                Register for hospital access.

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
                class="space-y-4"
                @submit.prevent="submit">

                <AppInput
                    v-model="form.userName"
                    label="Username"
                    placeholder="Username" />

                <div class="grid grid-cols-2 gap-4">

                    <AppInput
                        v-model="form.firstName"
                        label="First Name"
                        placeholder="First Name" />

                    <AppInput
                        v-model="form.lastName"
                        label="Last Name"
                        placeholder="Last Name" />

                </div>

                <AppInput
                    v-model="form.email"
                    label="Email"
                    type="email"
                    placeholder="Email" />

                <AppInput
                    v-model="form.phoneNumber"
                    label="Phone Number"
                    placeholder="Phone Number" />

                <AppInput
                    v-model="form.password"
                    label="Password"
                    type="password"
                    placeholder="Password" />

                <AppInput
                    v-model="form.confirmPassword"
                    label="Confirm Password"
                    type="password"
                    placeholder="Confirm Password" />

                <AppButton
                    :loading="loading">

                    Sign Up

                </AppButton>

            </form>

            <div
                class="
                mt-6
                text-center
                space-y-2">

                <p class="text-[#003049]">
                    Already have an account?
                </p>

                <RouterLink
                    to="/login"
                    class="
                    font-medium
                    text-[#003049]
                    hover:text-[#669BBC]">

                    ← Back to Login

                </RouterLink>

            </div>

        </template>

    </div>

</template>