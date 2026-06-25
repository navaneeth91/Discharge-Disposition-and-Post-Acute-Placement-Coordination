<script setup>
import {
    ref,
    computed,
    onMounted
}
from 'vue'

import HospitalLayout
from '@/layouts/HospitalLayout.vue'

import UserModal
from '@/components/admin/UserModal.vue'

import {
    useUserStore
}
from '@/stores/user'

const store =
    useUserStore()

const search =
    ref('')

const showModal =
    ref(false)

const selected =
    ref(null)

onMounted(() => {

    store.loadUsers()

})

const filtered =
    computed(() => {

        return store.users.filter(
            x =>

                x.firstName
                    ?.toLowerCase()
                    .includes(
                        search.value
                            .toLowerCase()
                    )

                ||

                x.email
                    ?.toLowerCase()
                    .includes(
                        search.value
                            .toLowerCase()
                    )
        )
    })

async function edit(id) {

    await store.loadUser(id)

    selected.value =
        store.selectedUser

    showModal.value =
        true
}

async function save(data) {

    await store.updateUser(
        selected.value.userId,
        data
    )

    showModal.value =
        false
}
</script>

<template>

<HospitalLayout>

<div class="space-y-6">

    <div
        class="
        bg-white
        rounded-3xl
        p-6
        shadow-md">

        <div
            class="
            flex
            justify-between
            mb-6">

            <h1
                class="
                text-3xl
                font-bold">

                Users

            </h1>

            <input
                v-model="search"
                placeholder="Search"
                class="
                border
                rounded-xl
                p-3
                w-80">
        </div>

        <table
            class="w-full">

            <thead>

                <tr>

                    <th>Name</th>

                    <th>Email</th>

                    <th>Department</th>

                    <th>Role</th>

                    <th></th>

                </tr>

            </thead>

            <tbody>

                <tr
                    v-for="
                    user in filtered"

                    :key="
                    user.userId"

                    class="
                    border-b
                    hover:bg-purple-50">

                    <td>

                        {{ user.firstName }}
                        {{ user.lastName }}

                    </td>

                    <td>

                        {{ user.email }}

                    </td>

                    <td>

                        {{ user.departmentName }}

                    </td>

                    <td>

                        {{ user.roleName }}

                    </td>

                    <td>

                        <button
                            @click="
                            edit(
                                user.userId
                            )"

                            class="
                            text-[#614083]">

                            Edit

                        </button>

                    </td>

                </tr>

            </tbody>

        </table>

    </div>

    <UserModal
        :show="showModal"
        :user="selected"
        @close="
        showModal=false"
        @save="save" />

</div>

</HospitalLayout>

</template>