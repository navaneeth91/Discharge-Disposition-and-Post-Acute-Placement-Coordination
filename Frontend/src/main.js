import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

import Toast, {
    POSITION
}
from 'vue-toastification'

import 'vue-toastification/dist/index.css'

import './assets/css/main.css'

const app =
    createApp(App)

app.use(createPinia())

app.use(router)

app.use(Toast, {

    position:
        POSITION.TOP_RIGHT,

    timeout: 3000,

    closeOnClick: true,

    pauseOnHover: true,

    draggable: true,

    newestOnTop: true,

    hideProgressBar: false

})

app.mount('#app')