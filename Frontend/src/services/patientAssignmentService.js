import api from '@/api/axios'

export const getUnassignedPatients = (
    page = 1,
    pageSize = 10,
    search = ''
) => {

    return api.get(
        '/PatientAssignment/unassigned-patients',
        {
            params: {
                page,
                pageSize,
                search
            }
        }
    )

}

export const getCareManagers = () => {

    return api.get(
        '/PatientAssignment/care-managers'
    )

}

export const assignCareManager = (payload) => {

    return api.post(
        '/PatientAssignment/assign',
        payload
    )

}