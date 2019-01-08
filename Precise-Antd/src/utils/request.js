import fetch from 'dva/fetch';
import { notification } from 'antd';
import router from 'umi/router';
import hash from 'hash.js';
import { isAntdPro } from './utils';
import globalService from './GlobalServices'
import AppConsts from './appconst'

const codeMessage = {
  200: '服务器成功返回请求的数据。',
  201: '新建或修改数据成功。',
  202: '一个请求已经进入后台排队（异步任务）。',
  204: '删除数据成功。',
  400: '发出的请求有错误，服务器没有进行新建或修改数据的操作。',
  401: '用户没有权限（令牌、用户名、密码错误）。',
  403: '用户得到授权，但是访问是被禁止的。',
  404: '发出的请求针对的是不存在的记录，服务器没有进行操作。',
  406: '请求的格式不可得。',
  410: '请求的资源被永久删除，且不会再得到的。',
  422: '当创建一个对象时，发生一个验证错误。',
  500: '服务器发生错误，请检查服务器。',
  502: '网关错误。',
  503: '服务不可用，服务器暂时过载或维护。',
  504: '网关超时。',
};

const checkStatus = response => {
  if (response.status >= 200 && response.status < 300)
    return response;
  else if (response.status == 401)
    return router.push('/accounts/login');
  else
    return response.json()
      .then(json => {
        if (response.ok) {
          return json
        } else {
          const errortext = codeMessage[response.status] || response.statusText;
          const error = new Error(errortext);
          if (json.success == false) {
            notification.error({
              message: `${json.error.message}`,
              description: json.error.details,
            });
          } else {
            notification.error({
              message: `请求错误 ${response.status}: ${response.url}`,
              description: errortext,
            });
            error.name = response.status;
          }
          error.response = response;
          throw error;
        }
      })
};

const cachedSave = (response, hashcode) => {
  /**
   * Clone a response data and store it in sessionStorage
   * Does not support data other than json, Cache only json
   */
  const contentType = response.headers.get('Content-Type');
  if (contentType && contentType.match(/application\/json/i)) {
    // All data is saved as text
    response
      .clone()
      .text()
      .then(content => {
        sessionStorage.setItem(hashcode, content);
        sessionStorage.setItem(`${hashcode}:timestamp`, Date.now());
      });
  }
  return response;
};

/**
 * 请求URL，返回承诺。
 *
 * @param  {string} url       要请求的网址
 * @param  {object} [option]  我们想要传递给“fetch”的选项
 * @return {object}           包含“data”或“err”的对象
 */
export default function request(url, option, parameter) {
  const options = {
    expirys: isAntdPro(),
    ...option,
  };
  if (parameter) {
    url += "?";
    for (var item in parameter) {
      if (item !== undefined && parameter[item] !== null && parameter[item] !== ""&& parameter[item] !== undefined)
        url += item + "=" + encodeURIComponent("" + parameter[item]) + "&";
    }
    url = url.replace(/[?&]$/, "");
  }
  /**
   * 根据网址和参数生成指纹
   * 也许url具有相同的参数
   */
  const fingerprint = url + (options.body ? JSON.stringify(options.body) : '');
  const hashcode = hash
    .sha256()
    .update(fingerprint)
    .digest('hex');

  const defaultOptions = {
    credentials: 'include',
  };
  const newOptions = { ...defaultOptions, ...options };
  const token = 'Bearer ' + globalService.auth.getToken();

  if (!(newOptions.body instanceof FormData)) {
    newOptions.headers = {
      Accept: 'application/json',
      'Authorization': token,
      'Content-Type': 'application/json; charset=utf-8',
      ...newOptions.headers,
    };
    newOptions.body = JSON.stringify(newOptions.body);
  } else {
    newOptions.headers = {
      'Authorization': token,
      Accept: 'application/json',
      ...newOptions.headers,
    };
  }

  const expirys = options.expirys && 60;
  // options.expirys !== false, return the cache,
  if (options.expirys !== false) {
    const cached = sessionStorage.getItem(hashcode);
    const whenCached = sessionStorage.getItem(`${hashcode}:timestamp`);
    if (cached !== null && whenCached !== null) {
      const age = (Date.now() - whenCached) / 1000;
      if (age < expirys) {
        const response = new Response(new Blob([cached]));
        return response.json();
      }
      sessionStorage.removeItem(hashcode);
      sessionStorage.removeItem(`${hashcode}:timestamp`);
    }
  }

  return fetch(url, newOptions)
    .then(checkStatus)
    .then(response => cachedSave(response, hashcode))
    .then(response => {
      // DELETE and 204 do not return data by default
      // using .json will report an error.
      if (newOptions.method === 'DELETE' || response.status === 204) {
        return response.text();
      }
      return response.json();
    })
    .catch(e => {
      const status = e.name;
      if (status === 401) {
        // @HACK
        /* eslint-disable no-underscore-dangle */
        window.g_app._store.dispatch({
          type: 'login/logout',
        });
        return;
      }
      // environment should not be used
      if (status === 403) {
        router.push('/exception/403');
        return;
      }
      if (status <= 504 && status >= 500) {
        router.push('/exception/500');
        return;
      }
      if (status >= 404 && status < 422) {
        router.push('/exception/404');
      }
    });
}