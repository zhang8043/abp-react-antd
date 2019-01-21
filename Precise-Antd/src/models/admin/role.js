import { getRoles, getRoleForEdit, createOrUpdateRole, deleteRole } from '@/services/admin/role';

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
        *getRoleForEdit({ payload }, { call, put }) {
            const response = yield call(getRoleForEdit, payload);
        },
        *createOrUpdateRole({ payload }, { call, put }) {
            const response = yield call(createOrUpdateRole, payload);
        },
        *deleteRole({ payload }, { call, put }) {
            const response = yield call(deleteRole, payload);
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
