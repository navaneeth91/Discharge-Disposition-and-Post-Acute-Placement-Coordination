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
        bg-white
        px-4
        py-3
        outline-none
        transition-all
        duration-300
        focus:ring-4"
        style="
        border-color: var(--border);
        "/>

    <p
      v-if="error"
      class="text-sm text-red-500">

      {{ error }}

    </p>

  </div>

</template>
<style scoped>

input:focus {
    border-color: #003049;
    box-shadow:
        0 0 0 4px
        rgba(102,155,188,.20);
}

</style>