import api from "@/api/axios";

export const getAssignedPatients = (search = "") => {
    return api.get(
        "/dispositions/assigned/patients",
        {
            params: {
                search
            }
        }
    );
};

export const createDispositionDecision = (decision) => {

    return api.post(
        "/dispositions/decisions",
        decision
    );

};
export const updateDispositionDecision = (
    decisionId,
    decision
) => {

    return api.put(
        `/dispositions/decisions/${decisionId}`,
        decision
    );

};

export const getPatientsByDepartment = (search = "") => {
    return api.get("/Patient/patients/DeptId", {
        params: {
            search
        }
    });
};

export const getPatientDetails = (patientId) => {

    return api.get(
        `/dispositions/decisionDetails/patient/${patientId}`
    );

};

