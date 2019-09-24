import modelExtend from 'dva-model-extend';
import { pageModel } from '@/utils/model';
import { pathMatchRegexp } from '@/utils/utils';
import { getUsers } from '@/services/admin/user';

export default modelExtend(pageModel, {
    namespace: 'users',

    state: {
        userSelectedRowKeys: []
    },

    subscriptions: {
        setup({ dispatch, history }) {
            history.listen(location => {
                if (pathMatchRegexp('/admin/user', location.pathname)) {
                    const payload = location.query || { page: 1, pageSize: 10 }
                    dispatch({
                        type: 'getUsers',
                        payload,
                    })
                }
            })
        },
    },

    effects: {
        *getUsers({ payload = {} }, { call, put }) {
            const response = yield call(getUsers, payload);
            if (response) {
                yield put({
                    type: 'querySuccess',
                    payload: {
                        list: response.result.items,
                        pagination: {
                            current: Number(payload.skipCount) || 0,
                            pageSize: Number(payload.maxResultCount) || 10,
                            total: response.result.totalCount,
                        },
                    },
                })
            }
        },
    },

    reducers: {

    },
})
