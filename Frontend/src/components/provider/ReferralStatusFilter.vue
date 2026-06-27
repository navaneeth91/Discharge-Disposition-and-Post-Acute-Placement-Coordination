<script setup>
import { ref, watch } from 'vue'

const emit = defineEmits([
    'filter-change'
])

const selectedStatus = ref('All')

watch(selectedStatus, () => {

    emit(
        'filter-change',
        selectedStatus.value
    )

})
</script>

<template>

<select
    v-model="selectedStatus"

    class="
    bg-white
    border
    border-[#D8D3C4]
    rounded-2xl
    px-4
    py-3
    text-[#003049]">

    <option value="All">
        All 
    </option>

    <option value="Pending">
        Pending
    </option>

    <option value="Approved">
        Approved
    </option>

    <option value="Denied">
        Denied
    </option>

</select>

</template>