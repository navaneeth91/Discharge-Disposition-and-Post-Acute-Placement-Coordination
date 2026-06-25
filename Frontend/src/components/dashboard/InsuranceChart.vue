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
    getInsuranceAnalytics
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
            label: 'Requests',

            data: [],

            backgroundColor: [
                '#003049',
                '#669BBC',
                '#C1121F',
                '#780000'
            ],

            borderRadius: 12,

            maxBarThickness: 70
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
                color: '#003049'
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
            await getInsuranceAnalytics()
        const insurance = response.data.data
        console.log(response.data)

        chartData.value.labels =
            insurance.map(
                x => x.serviceType
            )

        chartData.value.datasets[0].data =
            insurance.map(
                x => x.total
            )
    }
    catch (error) {

        console.error(
            'Insurance chart error:',
            error
        )
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
    shadow-lg">

    <h2
        class="
        text-xl
        font-semibold
        text-[#003049]
        mb-6">

        Service Type Distribution

    </h2>

    <div
        v-if="loading"
        class="
        h-[350px]
        animate-pulse
        rounded-2xl
        bg-slate-100">
    </div>

    <div
        v-else
        class="
        relative
        w-full
        h-[350px]
        lg:h-[400px]
        xl:h-[450px]">

        <Bar
            :data="chartData"
            :options="chartOptions" />

    </div>

</div>

</template>