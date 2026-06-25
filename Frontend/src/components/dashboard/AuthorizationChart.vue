<script setup>
import { ref, onMounted }
from 'vue'

import { Doughnut }
from 'vue-chartjs'

import {
    Chart as ChartJS,
    ArcElement,
    Tooltip,
    Legend
}
from 'chart.js'

import {
    getAuthorizationAnalytics
}
from '@/services/dashboardService'

ChartJS.register(
    ArcElement,
    Tooltip,
    Legend
)

const loading = ref(true)

const chartData = ref({

    labels: [],

    datasets: [
        {
            data: [],

            backgroundColor: [
                '#003049',
                '#669BBC',
                '#C1121F',
                '#780000'
            ],

            borderWidth: 0
        }
    ]
})

const chartOptions = {

    responsive: true,

    maintainAspectRatio: false,

    cutout: '55%',

    plugins: {

        legend: {

            position: 'bottom',

            labels: {
                padding: 20,
                color: '#003049'
            }
        }
    }
}

async function loadChart() {

    try {

        const response =
            await getAuthorizationAnalytics()

        const analytics =
            response.data.data

        chartData.value.labels =
            analytics.map(
                x => x.status
            )

        chartData.value.datasets[0].data =
            analytics.map(
                x => x.total
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

        Authorization Status

    </h2>

    <div
        v-if="loading"
        class="
        skeleton
        h-[300px]
        rounded-2xl">
    </div>

    <div
    class="
    h-[380px]
    flex
    items-center
    justify-center">

    <Doughnut
        v-if="!loading"
        :data="chartData"
        :options="chartOptions" />

    </div>

</div>

</template>