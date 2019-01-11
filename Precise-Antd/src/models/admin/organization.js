import { getOrganizationUnits,moveOrganizationUnit } from '@/services/admin/organization';
import { createTree } from '@/utils/utils';

export default {
    namespace: 'organization',

    state: {
        organizationData: [],
    },

    effects: {
        *getOrganizationUnits({ payload }, { call, put }) {
            const response = yield call(getOrganizationUnits, payload);
            yield put({
                type: 'save',
                payload: response.result.items,
            });
        },
        *moveOrganizationUnit({ payload }, { call, put }) {
            const response = yield call(moveOrganizationUnit, payload);
           console.log(response);
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
                            return item.id+"";
                        }
                    }
                ]
            );
            return {
                ...state,
                organizationData: data,
            };
        },
    },
};
