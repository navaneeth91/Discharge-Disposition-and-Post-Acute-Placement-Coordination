import api from '@/api/axios'

export const getHospitalDashboard = () => {
    return api.get('/dashboard/hospital')
}

export const getInsuranceDashboard = () => {
    return api.get('/dashboard/insurance')
}