import api from '@/api/axios'

export const getUsers = (
    page = 1,
    pageSize = 10,
    search = ''
) => {

    return api.get('/Admin/users', {

        params: {
            page,
            pageSize,
            search
        }
    })
}

export const getUser = (id) => {

    return api.get(
        `/Admin/users/${id}`
    )
}

export const updateUser = (
    id,
    data
) => {

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