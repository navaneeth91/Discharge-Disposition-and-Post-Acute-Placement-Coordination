import api from '@/api/axios'

export const getPatients = (
    page = 1,
    pageSize = 10,
    search = '',
    status = 'all'
) => {

    return api.get(
        '/Admin/patients',
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

export const getPatientById = (id) => {

    return api.get(
        `/Admin/patients/${id}`
    )
}

export const dischargePatient = (
    id,
    date
) => {

    return api.patch(
        `/Patient/${id}/discharge`,
        {
            actualDischargeDate: date
        }
    )
}