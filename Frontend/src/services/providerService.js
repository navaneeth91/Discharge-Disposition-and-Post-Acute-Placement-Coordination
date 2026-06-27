import api from '@/api/axios'



export function getProviderReferrals(userId,page,pageSize,search,status) {

    return api.get(
        `/referrals/provider/${userId}`,
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

export function acceptReferral(id,status) {

    return api.put(
        `/referrals/${id}/status?status=2`
    )
}

export function rejectReferral(id) {

    return api.put(
        `/referrals/${id}/status?status=3`
    )
}

export function getReferralDetails(referralId) {

    return api.get(
        `/referrals/details/${referralId}`
    )

}

