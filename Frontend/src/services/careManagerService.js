import api from '@/api/axios'

export async function getDashboard(careManagerId) {
    return await api.get(
        `/CareManager/dashboard/${careManagerId}`
    )
}

export async function getMyPatients(
    careManagerId,
    page = 1,
    pageSize = 10,
    search = ''
) {
    return await api.get(
        `/PatientAssignment/care-manager/${careManagerId}/patients`,
        {
            params: {
                page,
                pageSize,
                search
            }
        }
    )
}

export async function getProvidersByDisposition(
    dispositionTypeId
) {
    return await api.get(
        `/post-acute-providers/disposition/${dispositionTypeId}`
    )
}

export async function createReferral(data) {
    return await api.post(
        '/referrals',
        data
    )
}

export async function getReferralByPatient(
    patientId
) {
    return await api.get(
        `/referrals/patient/${patientId}`
    )
}
export async function getReferralTracking(
    careManagerId,
    page = 1,
    pageSize = 10,
    search = '',
    status = null
) {
    return await api.get(
        `/referrals/care-manager/${careManagerId}`,
        {
            params: {
                page,
                pageSize,
                search,
                status
            }
        }
    )
}
export async function getAuthorizationTracking(
    careManagerId,
    page = 1,
    pageSize = 10,
    search = '',
    status = null
) {
    return await api.get(
        `/authorizations/care-manager/${careManagerId}`,
        {
            params: {
                page,
                pageSize,
                search,
                status
            }
        }
    )
}
export async function createAuthorization(data) {
    return await api.post(
        '/authorizations',
        data
    )
}