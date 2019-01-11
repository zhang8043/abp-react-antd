import { getRoles } from '@/services/admin/role';

export default {
    namespace: 'role',

    state: {
        data: [],
    },

    effects: {
        *getRoles({ payload }, { call, put }) {
            const response = yield call(getRoles, payload);
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
