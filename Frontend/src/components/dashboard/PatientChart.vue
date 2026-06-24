<script setup>
import { Bar }
from 'vue-chartjs'

import {
    Chart as ChartJS,
    CategoryScale,
    LinearScale,
    BarElement,
    Tooltip,
    Legend
}
from 'chart.js'

ChartJS.register(
    CategoryScale,
    LinearScale,
    BarElement,
    Tooltip,
    Legend
)

const chartData = {
    labels: [
        'Cardiology',
        'ICU',
        'Orthopedics',
        'Medicine'
    ],

    datasets: [
        {
            label: 'Patients',

            data: [35, 20, 28, 40],

            backgroundColor: [
                '#003049',
                '#669BBC',
                '#C1121F',
                '#780000'
            ],

            borderRadius: 10
        }
    ]
}

const chartOptions = {

    responsive: true,

    plugins: {
        legend: {
            display: false
        }
    }
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

        Patients By Department

    </h2>

    <Bar
        :data="chartData"
        :options="chartOptions" />

</div>

</template>