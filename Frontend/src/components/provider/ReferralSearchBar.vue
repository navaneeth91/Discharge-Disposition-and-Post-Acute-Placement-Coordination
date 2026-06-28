<script setup>
import { ref, watch } from 'vue'

const emit = defineEmits([
    'search'
])

const searchText = ref('')

watch(searchText, () => {

    emit(
        'search',
        searchText.value
    )

})
</script>

<template>

<div>

    <input
        v-model="searchText"
        type="text"
        placeholder="Search by patient name..."
        class="
        w-full
        bg-white
        border
        border-[#D8D3C4]
        rounded-2xl
        p-4
        outline-none
        focus:border-[#669BBC]" />

</div>

</template>