<script setup>
import { ref, onMounted }
from 'vue'

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

import {
    getPatientDistribution
}
from '@/services/dashboardService'

ChartJS.register(
    CategoryScale,
    LinearScale,
    BarElement,
    Tooltip,
    Legend
)

const loading = ref(true)

const chartData = ref({
    labels: [],
    datasets: [
        {
            label: 'Patients',

            data: [],

            backgroundColor: [
                '#003049',
                '#669BBC',
                '#C1121F',
                '#780000',
                '#003049',
                '#669BBC'
            ],

            borderRadius: 10
        }
    ]
})

const chartOptions = {
    responsive: true,
    maintainAspectRatio: false,

    plugins: {
        legend: {
            display: false
        }
    },

    scales: {
        x: {
            ticks: {
                color: '#003049',
                maxRotation: 0,
                minRotation: 0
            },

            grid: {
                display: false
            }
        },

        y: {
            beginAtZero: true,

            ticks: {
                color: '#003049'
            }
        }
    }
}

async function loadChart() {

    try {

        const response =
            await getPatientDistribution()

        const patients =
            response.data.data

        chartData.value.labels =
            patients.map(
                x => x.departmentName
            )

        chartData.value.datasets[0].data =
            patients.map(
                x => x.totalPatients
            )
    }
    catch (error) {

        console.error(error)
    }
    finally {

        loading.value = false
    }
}

onMounted(loadChart)
</script>

<template>

<div
    class="
    bg-white
    rounded-3xl
    p-6
    shadow-lg
    h-[500px]">

    <h2
        class="
        text-xl
        font-semibold
        text-[#003049]
        mb-6">

        Patients By Department

    </h2>

    <div
        v-if="loading"
        class="
        skeleton
        h-[300px]
        rounded-2xl">
    </div>

    <div
    class="h-[380px]">

    <Bar
        v-if="!loading"
        :data="chartData"
        :options="chartOptions" />

    </div>

</div>

</template>