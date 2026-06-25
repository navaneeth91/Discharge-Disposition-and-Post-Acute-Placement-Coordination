import api from '@/api/axios'

export const getPatients = () => {
    return api.get('/Admin/patients')
}

export const getPatientById = (id) => {
    return api.get(`/Admin/patients/${id}`)
}

export const dischargePatient = (id, date) => {
    return api.patch(
        `/Patient/${id}/discharge`,
        {
            actualDischargeDate: date
        }
    )
}