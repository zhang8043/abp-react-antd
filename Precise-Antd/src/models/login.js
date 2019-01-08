import { routerRedux } from 'dva/router';
import { stringify } from 'qs';
import {  getFakeCaptcha } from '@/services/api';
import { setAuthority } from '@/utils/authority';
import { getPageQuery } from '@/utils/utils';
import { reloadAuthorized } from '@/utils/Authorized';
import { GetAll, getCurrentLoginInformation } from '@/services/sessionStore';
import appconst from '@/utils/appconst';
import { fakeAccountLogin } from '@/services/tokenAuth';
import globalService from '@/utils/GlobalServices'

export default {
  namespace: 'login',

  state: {
    status: undefined,
  },

  effects: {
    *login({ payload }, { call, put }) {
      const response = yield call(fakeAccountLogin, payload);
      yield put({
        type: 'changeLoginStatus',
        payload: response,
      });
      // 登录成功
      if (response != null && response.success) {
        // 是否要求重置密码
        if (response.result.shouldResetPassword) {
          console.log("需重置密码");
        }
        var tokenExpireDate = payload.rememberClient ? new Date(new Date().getTime() + 1000 * response.result.expireInSeconds) : undefined;
        globalService.auth.setToken(response.result.accessToken, tokenExpireDate);
        globalService.cookie.setCookieValue(appconst.authorization.encrptedAuthTokenName, response.result.encryptedAccessToken, tokenExpireDate, "/");
        const getAllResponse = yield call(GetAll);
        if (getAllResponse != null && getAllResponse.success) {
          globalService.auth.setAuthority(appconst.authorization.grantedAuthorityName, getAllResponse.result.auth.grantedPermissions);
        }
        const getSessionStore = yield call(getCurrentLoginInformation);
        if (getSessionStore != null && getSessionStore.success) {
          globalService.sessionStore.setSessionStore(getSessionStore.result);
        }
        reloadAuthorized();
        const urlParams = new URL(window.location.href);
        const params = getPageQuery();
        let { redirect } = params;
        if (redirect) {
          const redirectUrlParams = new URL(redirect);
          if (redirectUrlParams.origin === urlParams.origin) {
            redirect = redirect.substr(urlParams.origin.length);
            if (redirect.match(/^\/.*#/)) {
              redirect = redirect.substr(redirect.indexOf('#') + 1);
            }
          } else {
            window.location.href = redirect;
            return;
          }
        }
        yield put(routerRedux.replace(redirect || '/'));
      }
    },

    *getCaptcha({ payload }, { call }) {
      yield call(getFakeCaptcha, payload);
    },

    *logout(_, { put }) {
      yield put({
        type: 'changeLoginStatus',
        payload: {
          status: false,
          currentAuthority: 'guest',
        },
      });
      reloadAuthorized();
      yield put(
        routerRedux.push({
          pathname: '/accounts/login',
          search: stringify({
            redirect: window.location.href,
          }),
        })
      );
    },
  },

  reducers: {
    changeLoginStatus(state, { payload }) {
      return {
        ...state,
        status: payload != null ? payload.success : false,
        type: "account",
      };
    },
  },
};
