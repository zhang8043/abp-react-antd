import store from 'store';
import AppConsts from './appconst'
import moment from "moment";

const tokenKey = AppConsts.localization.defaultLocalizationSourceName;
const sessionStoreName = "PreciseSessionStore";

let globalService = {
    app: {

    },
    store: {
        get: function (key) {
            var cacheObj = store.get(key);
            if (cacheObj) {
                var now = new Date();

                if (!cacheObj.expire) return null;
                var expireTime = new Date(cacheObj.expire);
                if (expireTime < now) {//超出有效期,移除
                    globalService.store.remove(key);
                    return null;
                } else {
                    //有效期不足半小时,则刷新
                    if (expireTime - now < (30 * 60 * 1000)) {
                        cacheObj.expire = new Date((now / 1000 + 120 * 60) * 1000);
                        store.set(key, cacheObj)
                    }
                    return cacheObj.value;
                }
            }
            return null;
        },
        set: function (key, value, span) {
            var sp = span || 120;//默认缓存两小时

            var now = new Date();
            var expire = new Date((now / 1000 + sp * 60) * 1000);
            var cacheObj = {
                expire: expire,
                value: value
            };
            store.set(key, cacheObj);
        },
        remove: function (key) {
            store.remove(key);
        },
        clearAll: function () {
            store.clearAll();
        }
    },
    sessionStore: {
        setSessionStore: function (value) {
            localStorage.setItem(sessionStoreName, JSON.stringify(value));
        },
        getSessionStore: function () {
            const sessionStoreString = localStorage.getItem(sessionStoreName);
            let sessionStores;
            try {
                sessionStores = JSON.parse(sessionStoreString);
            } catch (e) {
                sessionStores = sessionStoreString;
            }
            if (typeof sessionStores === 'string') {
                return [sessionStores];
            }
            return sessionStores || [];
        },
        getItem: function (item) {
            const sessionStores = globalService.sessionStore.getSessionStore();
            let ret;
            if (sessionStores.length == 0) {
                return ret || [];
            }
            if (sessionStores[item] != null) {
                ret = sessionStores[item];
            }
            return ret;
        },
    },
    auth: {
        setAuthority: function (authorityName, authority) {
            const proAuthority = typeof authority === 'string' ? [authority] : authority;
            return localStorage.setItem(authorityName, JSON.stringify(proAuthority));
        },
        getAllAuthority: function (str) {
            const authorityString =
                typeof str === 'undefined' ? localStorage.getItem(AppConsts.authorization.allAuthorityName) : str;
            let authority;
            try {
                authority = JSON.parse(authorityString);
            } catch (e) {
                authority = authorityString;
            }
            if (typeof authority === 'string') {
                return [authority];
            }
            return authority || [];
        },
        getGrantedAuthority: function (str) {
            const authorityString =
                typeof str === 'undefined' ? localStorage.getItem(AppConsts.authorization.grantedAuthorityName) : str;
            let authority;
            try {
                authority = JSON.parse(authorityString);
            } catch (e) {
                authority = authorityString;
            }
            if (typeof authority === 'string') {
                return [authority];
            }
            return authority || [];
        },
        isGranted: function (permissionName) {
            return globalService.auth.getAllAuthority()[permissionName] != undefined && globalService.auth.getGrantedAuthority()[permissionName] != undefined;
        },
        isAnyGranted: function (args) {
            if (!args || args.length <= 0) {
                return true;
            }
            for (var i = 0; i < args.length; i++) {
                if (globalService.auth.isGranted(args[i])) {
                    return true;
                }
            }
            return false;
        },
        areAllGranted: function (args) {
            if (!args || args.length <= 0) {
                return true;
            }
            for (var i = 0; i < args.length; i++) {
                if (!globalService.auth.isGranted(args[i])) {
                    return false;
                }
            }
            return true;
        },
        getToken: function () {
            return globalService.cookie.getCookieValue(tokenKey);
        },
        setToken: function (token, expire) {
            globalService.cookie.setCookieValue(tokenKey, token, expire, "/");
        },
        clearToken: function () {
            globalService.store.clearAll();
            globalService.cookie.deleteCookie(tokenKey, "/");
            globalService.cookie.deleteCookie(AppConsts.authorization.encrptedAuthTokenName, "/");
        }
    },
    cookie: {
        setCookieValue: function (key, value, expireDate, path, domain) {
            var cookieValue = encodeURIComponent(key) + '=';
            if (value) {
                cookieValue = cookieValue + encodeURIComponent(value);
            }
            if (expireDate) {
                cookieValue = cookieValue + "; expires=" + expireDate.toUTCString();
            }
            if (path) {
                cookieValue = cookieValue + "; path=" + path;
            }
            if (domain) {
                cookieValue = cookieValue + "; domain=" + domain;
            }
            document.cookie = cookieValue;
        },
        getCookieValue: function (key) {
            var equalities = document.cookie.split('; ');
            for (var i = 0; i < equalities.length; i++) {
                if (!equalities[i]) {
                    continue;
                }
                var splitted = equalities[i].split('=');
                if (splitted.length != 2) {
                    continue;
                }
                if (decodeURIComponent(splitted[0]) === key) {
                    return decodeURIComponent(splitted[1] || '');
                }
            }
            return null;
        },
        deleteCookie: function (key, path) {
            var cookieValue = encodeURIComponent(key) + '=';
            cookieValue = cookieValue + "; expires=" + (new Date(new Date().getTime() - 86400000)).toUTCString();
            if (path) {
                cookieValue = cookieValue + "; path=" + path;
            }
            document.cookie = cookieValue;
        }
    }
}

export default globalService;
