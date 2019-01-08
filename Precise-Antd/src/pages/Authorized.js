import React from 'react';
import RenderAuthorized from '@/components/Authorized';
import { getAuthority } from '@/utils/authority';
import Redirect from 'umi/redirect';
import router from 'umi/router';
import globalService from '@/utils/GlobalServices';

const Authority = getAuthority();
const Authorized = RenderAuthorized(Authority);

if (globalService.sessionStore.getSessionStore().user == null) {
  router.push('/accounts/login');
}

export default ({ children }) => (
  <Authorized authority={children.props.route.authority} noMatch={<Redirect to="/accounts/login" />}>
    {children}
  </Authorized>
);
