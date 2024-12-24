import { createFetch } from '@vueuse/core';
import { useAuthStore } from '@/stores/auth';

export const useArvantFetch = createFetch({
  baseUrl: '/api',
  options: {
    async afterFetch(ctx) {
      const data = ctx.data;
      if (data.succeeded) {
        ctx.data = data.data ?? null;
      } else {
        ctx.data = null;
        //TODO: show notification
      }
      return ctx;
    },
    async onFetchError(ctx) {
      const { logout } = useAuthStore();
      if(ctx.response?.status === 401) {
        await logout();
      }
      return ctx;
    }
  },
  fetchOptions: {
    credentials: 'include'
  }
});
