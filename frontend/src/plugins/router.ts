import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: () => import('@/pages/Login.vue')
    },
    {
      path: '/',
      name: 'home',
      component: () => import('@/layout/layout.vue'),
      children: [
        {
          path: '/feed',
          component: () => import('@/pages/Feed.vue')
        },
        {
          path: '/calendar',
          component: () => import('@/pages/Calendar.vue')
        },
        {
          path: '/friends',
          component: () => import('@/pages/Friends.vue')
        },
        {
          path: '/calls',
          component: () => import('@/pages/Calls.vue')
        },
        {
          path: '/settings',
          redirect: { name: 'general' },
          children: [
            {
              path: 'general',
              component: () => import('@/pages/settings/General.vue')
            },
            {
              path: 'team',
              component: () => import('@/pages/settings/Team.vue')
            },
            {
              path: 'limits',
              component: () => import('@/pages/settings/Limits.vue')
            }
          ]
        }
      ]
    }
  ]
});

router.beforeEach(async (to, from, next) => {
  const authStore = useAuthStore();
  if (to.path !== '/login') {
    if (!authStore.isAuthenticated) {
      await authStore.fetchCurrentUser();
    }
    if (authStore.isAuthenticated) {
      next();
    } else {
      return next({ name: 'login' });
    }
  } else {
    next();
  }
});

export default router;
