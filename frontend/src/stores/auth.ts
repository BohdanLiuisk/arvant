import { defineStore } from 'pinia';
import { useArvantFetch } from '@/composables/use-arvant-fetch';
import router from '@/plugins/router';
import { useFetch } from '@vueuse/core';
import type { BackendResult } from '@/types/backend-result';
import type { User } from '@/types/user';

export interface AuthState {
  isAuthenticated: boolean;
  user: User | null;
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    isAuthenticated: false,
    user: null
  }),
  actions: {
    async login(username: string, password: string) {
      const { data } = await useFetch('/api/users/loginCookie').post({
        email: username,
        password: password
      }).json<BackendResult>();
      if(data.value?.succeeded) {
        await this.fetchCurrentUser();
        await router.push('/');
      } else {

      }
    },
    async fetchCurrentUser() {
      try {
        const { data } = await useArvantFetch('users/getCurrentUser').get().json<User>();
        if (data.value) {
          this.isAuthenticated = true;
          this.user = data.value;
        } else {
          await this.logout();
        }
      } catch (error) {
        await this.logout();
      }
    },
    async logout() {
      this.isAuthenticated = false;
      this.user = null;
      await router.push('/login');
    }
  }
});
