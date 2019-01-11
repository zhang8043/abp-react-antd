import { getAllPermissions } from '@/services/admin/permission';

export default {
    namespace: 'permission',

    state: {
        allPermissions: [],
    },

    effects: {
        *getAllPermissions({ payload }, { call, put }) {
            const response = yield call(getAllPermissions, payload);
            yield put({
                type: 'save',
                payload: response.result.items,
            });
        },
    },

    reducers: {
        save(state, action) {
            return {
                ...state,
                allPermissions: action.payload,
            };
        },
    },
};
