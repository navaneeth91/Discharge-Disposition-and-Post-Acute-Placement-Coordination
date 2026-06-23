<script setup>
defineProps({
  modelValue: String,
  type: {
    type: String,
    default: 'text'
  },
  label: String,
  placeholder: String,
  error: String
})

defineEmits(['update:modelValue'])
</script>

<template>

  <div class="space-y-2">

    <label
      class="text-sm font-medium text-slate-700">

      {{ label }}

    </label>

    <input
      :type="type"
      :placeholder="placeholder"
      :value="modelValue"
      @input="$emit(
        'update:modelValue',
        $event.target.value
      )"
      class="
w-full
rounded-xl
border
border-slate-300
bg-white
px-4
py-3
outline-none
transition
duration-300
focus:border-[#79599B]
focus:ring-4
focus:ring-purple-100"/>

    <p
      v-if="error"
      class="text-sm text-red-500">

      {{ error }}

    </p>

  </div>

</template>