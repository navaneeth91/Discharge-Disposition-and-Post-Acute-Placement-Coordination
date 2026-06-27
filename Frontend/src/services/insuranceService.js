import api from '@/api/axios'

export const getProviders = () => {
    return api.get('/insurance/providers')
}

export const getPlans = (providerId) => {
    return api.get('/insurance/plans', {
        params: {
            providerId: providerId || undefined
        }
    })
}