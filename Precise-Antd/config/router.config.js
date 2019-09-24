export default [
  // user
  {
    path: '/accounts',
    component: '../layouts/UserLayout',
    routes: [
      { path: '/accounts', redirect: '/Account/Login/login' },
      { path: '/accounts/login', component: './Account/Login/Login' },
      { path: '/accounts/register', component: './Account/Register/Register' },
      { path: '/accounts/register-result', component: './Account/RegisterResult/RegisterResult' },
    ],
  },
  // app
  {
    path: '/',
    component: '../layouts/BasicLayout',
    Routes: ['src/pages/Authorized'],
    routes: [
      // dashboard
      { path: '/', redirect: '/analysis' },
      {
        path: '/analysis',
        name: 'analysis',
        icon: 'precise-shouye',
        component: './Dashboard/Analysis',
      },
      {
        path: '/dashboard',
        name: 'dashboard',
        icon: 'precise-gongzuotai',
        routes: [
          {
            path: '/dashboard/workplace',
            name: 'workplace',
            icon: 'precise-zhuzhuangtu',
            component: './Dashboard/Workplace',
          },
          {
            path: '/dashboard/monitor',
            name: 'monitor',
            icon: 'precise-jiankong',
            component: './Dashboard/Monitor',
          },
        ],
      },
      // arcgis
      {
        path: '/arcgis',
        icon: 'precise-17',
        name: 'arcgis',
        routes: [
          {
            path: '/arcgis/arcgismap',
            name: 'arcgismap',
            icon: 'precise-changyongshili',
            component: './ArcgisMap',
          },
        ],
      },
      // list
      {
        path: '/list',
        icon: 'table',
        name: 'list',
        routes: [
          {
            path: '/list/table-list',
            name: 'searchtable',
            component: './List/TableList',
          },
          {
            path: '/list/basic-list',
            name: 'basiclist',
            component: './List/BasicList',
          },
          {
            path: '/list/card-list',
            name: 'cardlist',
            component: './List/CardList',
          },
          {
            path: '/list/search',
            name: 'searchlist',
            component: './List/List',
            routes: [
              {
                path: '/list/search',
                redirect: '/list/search/articles',
              },
              {
                path: '/list/search/articles',
                name: 'articles',
                component: './List/Articles',
              },
              {
                path: '/list/search/projects',
                name: 'projects',
                component: './List/Projects',
              },
              {
                path: '/list/search/applications',
                name: 'applications',
                component: './List/Applications',
              },
            ],
          },
        ],
      },
      {
        path: '/admin',
        name: 'admin',
        authority: ['Pages.Administration'],
        icon: 'precise-xitongguanli',
        routes: [
          {
            path: '/admin/organization',
            name: 'organization',
            authority: ['Pages.Administration.OrganizationUnits'],
            icon: 'cluster',
            component: './Admin/OrganizationUnit',
          },
          {
            path: '/admin/role',
            name: 'role',
            authority: ['Pages.Administration.Roles'],
            icon: 'idcard',
            component: './Admin/Role',
          },
          {
            path: '/admin/user',
            name: 'user',
            authority: ['Pages.Administration.Users'],
            icon: 'user',
            component: './Precise/Users',
          },
          {
            path: '/admin/workflow',
            name: 'workflow',
            icon: 'precise-navicon-lcpz',
            component: './Admin/WorkFlow/EditWork',
          },
          {
            path: '/admin/auditLog',
            name: 'auditLog',
            authority: ['Pages.Administration.AuditLogs'],
            icon: 'precise-rizhi',
            component: './Admin/AuditLog',
          },
          {
            path: '/admin/ui',
            name: 'ui',
            authority: ['Pages.Administration.UiCustomization'],
            icon: 'precise-yanjing',
            component: './Admin/UiCustomization',
          },
          {
            path: '/admin/settings',
            name: 'settings',
            authority: ['Pages.Administration.Tenant.Settings', 'Pages.Administration.Host.Settings'],
            icon: 'precise-shezhi',
            component: './Admin/Settings',
          },
        ],
      },
      {
        path: '/account',
        routes: [
          {
            path: '/account/center',
            name: 'center',
            component: './Account/Center/Center',
            routes: [
              {
                path: '/account/center',
                redirect: '/account/center/articles',
              },
              {
                path: '/account/center/articles',
                component: './Account/Center/Articles',
              },
              {
                path: '/account/center/applications',
                component: './Account/Center/Applications',
              },
              {
                path: '/account/center/projects',
                component: './Account/Center/Projects',
              },
            ],
          },
          {
            path: '/account/settings',
            name: 'settings',
            component: './Account/Settings/Info',
            routes: [
              {
                path: '/account/settings',
                redirect: '/account/settings/base',
              },
              {
                path: '/account/settings/base',
                component: './Account/Settings/BaseView',
              },
              {
                path: '/account/settings/security',
                component: './Account/Settings/SecurityView',
              },
              {
                path: '/account/settings/binding',
                component: './Account/Settings/BindingView',
              },
              {
                path: '/account/settings/notification',
                component: './Account/Settings/NotificationView',
              },
            ],
          },
        ],
      },
      {
        path: '/exception',
        routes: [
          // exception
          {
            path: '/exception/403',
            name: 'not-permission',
            component: './Exception/403',
          },
          {
            path: '/exception/404',
            name: 'not-find',
            component: './Exception/404',
          },
          {
            path: '/exception/500',
            name: 'server-error',
            component: './Exception/500',
          },
          {
            path: '/exception/trigger',
            name: 'trigger',
            hideInMenu: true,
            component: './Exception/TriggerException',
          },
        ],
      },
      {
        component: '404',
      },
    ],
  },
];
