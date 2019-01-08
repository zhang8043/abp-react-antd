import { getUsers } from '@/services/admin/user';

export default {
    namespace: 'user',

    state: {
        data: [],
    },

    effects: {
        *getUsers({ payload }, { call, put }) {
            const response = yield call(getUsers, payload);
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
