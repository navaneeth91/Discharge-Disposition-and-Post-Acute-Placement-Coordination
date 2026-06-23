<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import * as authService from '@/services/authService'

const router = useRouter()
const authStore = useAuthStore()

const loading = ref(false)

const form = reactive({
  email: '',
  password: ''
})

const error = ref('')

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
        router.push('/administrator')
        break

      case 'Physician':
        router.push('/physician')
        break

      case 'Care Manager':
        router.push('/care-manager')
        break

      case 'Post-Acute Provider':
        router.push('/provider')
        break

      case 'Authorization Coordinator':
        router.push('/insurance')
        break

      default:
        router.push('/login')
    }
  }
  catch (err) {
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
    class="w-full max-w-md bg-white rounded-3xl shadow-xl p-10 animate-fade"
  >
    <h2
      class="text-3xl font-bold text-slate-800 mb-2"
    >
      Welcome Back
    </h2>

    <p
      class="text-slate-500 mb-8"
    >
      Sign in to continue
    </p>

    <div
      v-if="error"
      class="bg-red-100 text-red-600 p-3 rounded mb-4"
    >
      {{ error }}
    </div>

    <form
      @submit.prevent="submit"
      class="space-y-5"
    >
      <div>
        <label class="block mb-2">
          Email
        </label>

        <input
          v-model="form.email"
          type="email"
          class="w-full border rounded-lg p-3"
        />
      </div>

      <div>
        <label class="block mb-2">
          Password
        </label>

        <input
          v-model="form.password"
          type="password"
          class="w-full border rounded-lg p-3"
        />
      </div>

      <button
        :disabled="loading"
        class="w-full bg-blue-600 hover:bg-blue-700 transition text-white p-3 rounded-lg"
      >
        {{ loading ? 'Signing In...' : 'Login' }}
      </button>
    </form>

    <div
      class="mt-6 text-center"
    >
      <RouterLink
        to="/signup"
        class="text-blue-600"
      >
        Create Account
      </RouterLink>
    </div>
  </div>
</template>