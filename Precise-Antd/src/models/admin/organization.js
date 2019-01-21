import {
    getOrganizationUnits,
    getOrganizationUnitUsers,
    moveOrganizationUnit,
    updateOrganizationUnit,
    createOrganizationUnit,
    deleteOrganizationUnit,
    removeUserFromOrganizationUnit,
    addUsersToOrganizationUnit,
    findUsers
} from '@/services/admin/organization';
import { createTree } from '@/utils/utils';
import { message } from 'antd';

export default {
    namespace: 'organization',

    state: {
        organizationData: [],
        organizationUnitUsersData: [],
        findUsersData:[]
    },

    effects: {
        *getOrganizationUnits(_, { call, put }) {
            const response = yield call(getOrganizationUnits);
            yield put({
                type: 'save',
                payload: response.result.items,
            });
        },
        *getOrganizationUnitUsers({ payload }, { call, put }) {
            const response = yield call(getOrganizationUnitUsers, payload);
            if (response.success) {
                yield put({
                    type: 'saveUser',
                    payload: response.result,
                });
            }
        },
        *createOrganizationUnit({ payload }, { call, put }) {
            const response = yield call(createOrganizationUnit, payload);
            if (response.success) {
                yield put({
                    type: 'getOrganizationUnits',
                });
                message.success('添加成功');
            }
        },
        *updateOrganizationUnit({ payload }, { call, put }) {
            const response = yield call(updateOrganizationUnit, payload);
            if (response.success) {
                yield put({
                    type: 'getOrganizationUnits',
                });
                message.success('修改成功');
            }
        },
        *moveOrganizationUnit({ payload }, { call, put }) {
            const response = yield call(moveOrganizationUnit, payload);
            if (response) {
                yield put({
                    type: 'getOrganizationUnits',
                });
                message.success('操作成功');
            }
        },
        *deleteOrganizationUnit({ payload }, { call, put }) {
            const response = yield call(deleteOrganizationUnit, payload);
            if (response) {
                yield put({
                    type: 'getOrganizationUnits',
                });
                message.success('操作成功');
            }
        },
        *removeUserFromOrganizationUnit({ payload, callback }, { call, put }) {
            const response = yield call(removeUserFromOrganizationUnit, payload);
            if (callback && typeof callback === 'function') {
                callback(response);
            }
        },
        *addUsersToOrganizationUnit({ payload, callback }, { call, put }) {
            const response = yield call(addUsersToOrganizationUnit, payload);
            if (response) {
                if (callback && typeof callback === 'function') {
                    callback(response);
                }
                yield put({
                    type: 'getOrganizationUnits',
                });
            }
        },
        *findUsers({ payload, callback }, { call, put }) {
            const response = yield call(findUsers, payload);
            if (response) {
                yield put({
                    type: 'saveFindUsers',
                    payload: response.result,
                });
            }
        },

    },

    reducers: {
        save(state, action) {
            function generateTextOnTree(ou) {
                return ou.displayName + '(' + ou.memberCount + ')';
            }

            let data = createTree(action.payload, 'parentId', 'id', null, 'children',
                [
                    {
                        target: 'displayName',
                        targetFunction(item) {
                            return generateTextOnTree(item);
                        }
                    },
                    {
                        target: 'key',
                        targetFunction(item) {
                            return item.id + "";
                        }
                    }
                ]
            );
            return {
                ...state,
                organizationData: data,
            };
        },
        saveUser(state, action) {
            return {
                ...state,
                organizationUnitUsersData: action.payload,
            };
        },
        saveFindUsers(state, action){
            return {
                ...state,
                findUsersData: action.payload,
            };
        }
    },
};
