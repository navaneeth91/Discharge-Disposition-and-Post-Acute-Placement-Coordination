import api from '@/api/axios'

export const searchMembers = (query, take = 20) => {
    return api.get('/members/search', {
        params: {
            query,
            take
        }
    })
}

export const getMemberById = (memberId) => {
    return api.get(`/members/${memberId}`)
}