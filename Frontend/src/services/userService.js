import api from '@/api/axios'

export const getUsers = () => {
    return api.get('/Admin/users')
}

export const getUser = (id) => {
    return api.get(`/Admin/users/${id}`)
}

export const updateUser = (id, data) => {
    return api.put(
        `/Admin/users/${id}`,
        data
    )
}

export const deleteUser = (id) => {
    return api.delete(
        `/Admin/users/${id}`
    )
}