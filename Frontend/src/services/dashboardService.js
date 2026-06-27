import api from '@/api/axios'


export const getHospitalDashboard = () => {
    return api.get('/dashboard/hospital')
}

export const getInsuranceDashboard = () => {
    return api.get('/insurance-dashboard/insurance')
}

export const getPatientDistribution = () => {
    return api.get(
        '/dashboard/patient-distribution'
    )
}

export const getAuthorizationAnalytics = () => {
    return api.get(
        '/dashboard/authorization-analytics'
    )
}

export const getInsuranceAnalytics = () => {
    return api.get(
        '/insurance-dashboard/service-analytics'
    )
}
export const getRecentInsuranceAuthorizations = (take = 10) => {
    return api.get('/insurance-authorizations/recent', {
        params: { take }
    })
}