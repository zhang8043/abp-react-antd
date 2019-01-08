import { getAuditLogs } from '@/services/admin/auditLog';

export default {
    namespace: 'auditlog',

    state: {
        data: [],
    },

    effects: {
        *getAuditLogs({ payload }, { call, put }) {
            const response = yield call(getAuditLogs, payload);
            yield put({
                type: 'save',
                payload: response,
            });
        },
    },

    reducers: {
        save(state, action) {
            return {
                ...state,
                data: action.payload.result,
            };
        },
    },
};
