<template>
  <div
    class="
      grid
      grid-cols-1
      sm:grid-cols-2
      xl:grid-cols-4
      gap-6">

    <div
      v-for="card in stats"
      :key="card.title"
      class="
        bg-white
        rounded-2xl
        shadow-lg
        p-6
        hover:-translate-y-1
        transition-all
        duration-300">

      <div class="flex justify-between items-start">

        <div>

          <p
            class="text-sm font-medium"
            style="color: var(--text-secondary);">

            {{ card.title }}

          </p>

          <h2
            class="text-3xl font-bold mt-2"
            style="color: var(--primary);">

            {{ card.value }}

          </h2>

          <p
            class="text-sm mt-2 font-medium"
            :style="{ color: card.textColor }">

            {{ card.description }}

          </p>

        </div>

        <div
          class="
            w-14
            h-14
            rounded-2xl
            flex
            items-center
            justify-center"
          :style="{ background: card.background }">

          <component
            :is="card.icon"
            :size="28"
            color="white" />

        </div>

      </div>

    </div>

  </div>
</template>

<script setup>

import {
    Users,
    ClipboardCheck,
    CheckCircle2,
    CalendarDays
} from "lucide-vue-next";

const stats = [

    {
        title: "Assigned Patients",
        value: 18,
        description: "+2 Since Yesterday",
        icon: Users,
        background: "var(--primary)",
        textColor: "var(--success)"
    },

    {
        title: "Pending Decisions",
        value: 4,
        description: "Requires Attention",
        icon: ClipboardCheck,
        background: "var(--warning)",
        textColor: "var(--warning)"
    },

    {
        title: "Completed Today",
        value: 12,
        description: "Excellent Progress",
        icon: CheckCircle2,
        background: "var(--success)",
        textColor: "var(--success)"
    },

    {
        title: "Expected Discharges",
        value: 6,
        description: "Scheduled Today",
        icon: CalendarDays,
        background: "var(--secondary)",
        textColor: "var(--secondary)"
    }

];

</script>