import api from '@/api/axios'

export const getAuthorizations = ({
    search = '',
    status = '',
    page = 1,
    pageSize = 10
} = {}) => {
    return api.get('/insurance-authorizations', {
        params: {
            search: search || undefined,
            status: status || undefined,
            page,
            pageSize
        }
    })
}

export const getRecentAuthorizations = (take = 5) => {
    return api.get('/insurance-authorizations/recent', {
        params: { take }
    })
}

export const getAuthorizationById = (authorizationRequestId) => {
    return api.get(`/insurance-authorizations/${authorizationRequestId}`)
}

export const updateAuthorizationStatus = (
    authorizationRequestId,
    payload
) => {
    return api.patch(
        `/insurance-authorizations/${authorizationRequestId}/status`,
        payload
    )
}