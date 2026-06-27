<script setup>
import {
    ref,
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


async function performSearch() {


    await store.searchUsers(
        search.value
    )
}


async function edit(id) {


    await store.loadUser(id)


    selected.value =
        store.selectedUser


    showModal.value =
        true
}


async function save(data) {


    console.log(data)


    console.log(
        selected.value.userId
    )


    await store.updateUser(
        selected.value.userId,
        data
    )


    showModal.value = false
}


function initials(user) {


    return (
        user.firstName?.charAt(0) +
        user.lastName?.charAt(0)
    ).toUpperCase()
}
</script>


<template>


<HospitalLayout>


<div class="space-y-6 fade-up">


    <div
        class="
        bg-white
        rounded-3xl
        p-8
        shadow-lg">


        <div
            class="
            flex
            flex-col
            md:flex-row
            md:justify-between
            md:items-center
            gap-4
            mb-8">


            <div>


                <h1
                    class="
                    text-3xl
                    font-bold
                    text-[#003049]">


                    Users


                </h1>


                <p
                    class="
                    text-gray-500
                    mt-1">


                    {{
                        store.totalCount
                    }}
                    registered users


                </p>


            </div>


            <input
                v-model="search"
                @input="performSearch"
                placeholder="Search users..."


                class="
                w-full
                md:w-80
                px-4
                py-3
                rounded-xl
                border
                border-gray-200
                outline-none
                focus:ring-4
                focus:ring-blue-100">
        </div>


        <div
            class="
            overflow-hidden
            rounded-2xl
            border
            border-gray-100">


            <table class="w-full">


                <thead
                    class="
                    bg-gray-50">


                    <tr>


                        <th
                            class="
                            text-left
                            px-6
                            py-4
                            font-semibold">


                            User


                        </th>


                        <th
                            class="
                            text-left
                            px-6
                            py-4
                            font-semibold">


                            Department


                        </th>


                        <th
                            class="
                            text-left
                            px-6
                            py-4
                            font-semibold">


                            Role


                        </th>


                        <th
                            class="
                            text-left
                            px-6
                            py-4
                            font-semibold">


                            Created


                        </th>


                        <th></th>


                    </tr>


                </thead>


                <tbody>


                    <tr
                        v-for="
                        user in
                        store.users"


                        :key="
                        user.userId"


                        class="
                        border-t
                        hover:bg-blue-50
                        transition">


                        <td
                            class="
                            px-6
                            py-5">


                            <div
                                class="
                                flex
                                items-center
                                gap-4">


                                <div
                                    class="
                                    w-12
                                    h-12
                                    rounded-full
                                    bg-[#003049]
                                    text-white
                                    flex
                                    items-center
                                    justify-center
                                    font-semibold">


                                    {{
                                        initials(user)
                                    }}


                                </div>


                                <div>


                                    <div
                                        class="
                                        font-semibold
                                        text-[#003049]">


                                        {{
                                            user.firstName
                                        }}
                                        {{
                                            user.lastName
                                        }}


                                    </div>


                                    <div
                                        class="
                                        text-sm
                                        text-gray-500">


                                        {{
                                            user.email
                                        }}


                                    </div>


                                </div>


                            </div>


                        </td>


                        <td
                            class="
                            px-6
                            py-5">


                            <span
                                class="
                                px-3
                                py-1
                                rounded-full
                                bg-blue-100
                                text-blue-700
                                text-sm">


                                {{
                                    user.departmentName
                                }}


                            </span>


                        </td>


                        <td
                            class="
                            px-6
                            py-5">


                            <span
                                class="
                                px-3
                                py-1
                                rounded-full
                                bg-green-100
                                text-green-700
                                text-sm">


                                {{
                                    user.roleName
                                }}


                            </span>


                        </td>


                        <td
                            class="
                            px-6
                            py-5
                            text-gray-500">


                            {{
                                new Date(
                                    user.createdAt
                                )
                                .toLocaleDateString()
                            }}


                        </td>


                        <td
                            class="
                            px-6
                            py-5
                            text-right">


                            <button
                                @click="
                                edit(
                                    user.userId
                                )"


                                class="
                                px-4
                                py-2
                                rounded-xl
                                bg-[#003049]
                                text-white
                                hover:bg-[#669BBC]
                                transition">


                                Edit


                            </button>


                        </td>


                    </tr>


                    <tr
                        v-if="
                        !store.loading &&
                        store.users.length === 0">


                        <td
                            colspan="5"
                            class="
                            text-center
                            py-10
                            text-gray-500">


                            No users found.


                        </td>


                    </tr>


                </tbody>


            </table>


        </div>


        <div
            class="
            flex
            flex-col
            md:flex-row
            justify-between
            items-center
            gap-4
            mt-8">


            <div
                class="
                text-sm
                text-gray-500">


                Page
                {{ store.page }}
                of
                {{ store.totalPages }}


            </div>


            <div
                class="
                flex
                gap-2">


                <button
                    @click="
                    store.previousPage()"


                    :disabled="
                    store.page === 1"


                    class="
                    px-4
                    py-2
                    rounded-xl
                    border
                    disabled:opacity-50">


                    Previous


                </button>


                <button
                    v-for="
                    page in
                    store.totalPages"


                    :key="page"


                    @click="
                    store.goToPage(
                        page
                    )"


                    :class="[
                        page ===
                        store.page
                        ? 'bg-[#003049] text-white'
                        : 'bg-gray-100',


                        'w-10 h-10 rounded-lg transition'
                    ]">


                    {{ page }}


                </button>


                <button
                    @click="
                    store.nextPage()"


                    :disabled="
                    store.page ===
                    store.totalPages"


                    class="
                    px-4
                    py-2
                    rounded-xl
                    border
                    disabled:opacity-50">


                    Next


                </button>


            </div>


        </div>


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
