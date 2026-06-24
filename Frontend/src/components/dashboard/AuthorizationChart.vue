<script setup>
import { Doughnut }
from 'vue-chartjs'

import {
    Chart as ChartJS,
    ArcElement,
    Tooltip,
    Legend
}
from 'chart.js'

ChartJS.register(
    ArcElement,
    Tooltip,
    Legend
)
const chartData = {

    labels: [
        'Approved',
        'Pending',
        'Denied'
    ],

    datasets: [
        {
            data: [20, 12, 5],

            backgroundColor: [
                '#003049',
                '#669BBC',
                '#C1121F'
            ],

            borderWidth: 0
        }
    ]
}

const chartOptions = {
    responsive: true
}
</script>

<template>

<div
    class="
    bg-white
    rounded-3xl
    p-6
    shadow-lg">

    <h2
        class="
        text-xl
        font-semibold
        text-[#003049]
        mb-6">

        Authorization Status

    </h2>

    <Doughnut
        :data="chartData"
        :options="chartOptions" />

</div>

</template>