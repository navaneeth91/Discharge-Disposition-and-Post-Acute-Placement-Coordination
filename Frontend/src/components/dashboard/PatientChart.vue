<script setup>
import {
    Bar
} from 'vue-chartjs'

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
        'Medicine',
        'ICU',
        'Cardiology',
        'Orthopedics'
    ],

    datasets: [
        {
            label: 'Patients',

            backgroundColor: '#79599B',

            data: [35, 18, 28, 14]
        }
    ]
}
</script>

<template>

    <div
        class="
        bg-white
        rounded-3xl
        p-6
        shadow-md">

        <h2
            class="
            text-xl
            font-semibold
            mb-5">

            Patients By Department

        </h2>

        <Bar
            :data="chartData" />

    </div>

</template>