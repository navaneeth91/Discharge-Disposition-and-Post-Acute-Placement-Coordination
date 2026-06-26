import api from '@/api/axios'

export const getReferrals = (
    page = 1,
    pageSize = 10,
    search = '',
    status = 'all'
) => {

    return api.get(
        '/referrals',
        {
            params: {
                page,
                pageSize,
                search,
                status:
                    status === 'all'
                        ? null
                        : status
            }
        }
    )
}

export const getReferral = id => {

    return api.get(
        `/referrals/${id}`
    )
}

export const updateStatus = (
    id,
    status
) => {

    return api.put(
        `/referrals/${id}/status`,
        null,
        {
            params: {
                status
            }
        }
    )
}