<script setup>
import { reactive, watch }
from 'vue'

const props =
    defineProps({

        user: Object,

        show: Boolean
    })

const emit =
    defineEmits([
        'close',
        'save'
    ])

const form =
    reactive({

        firstName: '',

        lastName: '',

        phoneNumber: '',

        email: '',

        roleId: 1,

        deptId: 1
    })

watch(
    () => props.user,

    value => {

        if (!value)
            return

        form.firstName =
            value.firstName

        form.lastName =
            value.lastName

        form.phoneNumber =
            value.phoneNumber

        form.email =
            value.email

        form.roleId =
            value.roleId

        form.deptId =
            value.deptId
    }
)
</script>

<template>

<Transition name="slide">

<div
    v-if="show"
    class="
    fixed
    inset-0
    bg-black/30
    z-50
    flex
    justify-end">

    <div
        class="
        w-[500px]
        h-full
        bg-white
        p-8
        shadow-2xl">

        <div
            class="
            flex
            justify-between
            mb-8">

            <h2
                class="
                text-3xl
                font-bold
                text-[#2D1E3E]">

                Edit User

            </h2>

            <button
                @click="$emit('close')">

                ✕

            </button>

        </div>

        <div class="space-y-4">

            <input
                v-model="form.firstName"
                placeholder="First Name"
                class="input">

            <input
                v-model="form.lastName"
                placeholder="Last Name"
                class="input">

            <input
                v-model="form.phoneNumber"
                placeholder="Phone"
                class="input">

            <input
                v-model="form.email"
                placeholder="Email"
                class="input">

            <select
                v-model="form.roleId"
                class="input">

                <option :value="1">
                    Administrator
                </option>

                <option :value="2">
                    Physician
                </option>

                <option :value="3">
                    Care Manager
                </option>

                <option :value="4">
                    Post-Acute Provider
                </option>

                <option :value="5">
                    Authorization Coordinator
                </option>

            </select>

            <input
                v-model="form.deptId"
                type="number"
                class="input">

            <button
                @click="
                emit(
                    'save',
                    form
                )"

                class="
                w-full
                mt-6
                py-3
                rounded-xl
                bg-[#614083]
                text-white
                hover:bg-[#53366F]
                transition">

                Save Changes

            </button>

        </div>

    </div>

</div>

</Transition>

</template>

<style scoped>
.input {
    width: 100%;
    border: 1px solid #DDD2E8;
    border-radius: 12px;
    padding: 12px;
}

.slide-enter-active,
.slide-leave-active {
    transition: .3s;
}

.slide-enter-from,
.slide-leave-to {
    transform: translateX(100%);
}
</style>