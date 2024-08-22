import TokenService from '~/scripts/tokenService';

export default defineNuxtRouteMiddleware((to, from) => {
  const tokenService = new TokenService();
  switch (to.path) {
    case '/announcementEdit':
      if (!tokenService.isAdmin()) {
        return navigateTo('/403');
      }
      break;
  }
});
