<script setup lang="ts">
import { RouterView, useRouter } from 'vue-router';
import { computed, ref } from 'vue';
import {
  NConfigProvider,
  NNotificationProvider,
  NMessageProvider,
  NSpin,
  darkTheme,
  type GlobalThemeOverrides,
  lightTheme
} from 'naive-ui';
import { useNaiveDesignTokens } from '@/composables/use-design-tokens';
import { useAppearanceStore } from '@/stores/appearance';

const { commonTokens } = useNaiveDesignTokens();
const { isDark } = useAppearanceStore();
const tokenTheme = computed(() => (isDark.value ? darkTheme : lightTheme));
const themeOverrides = computed((): GlobalThemeOverrides => {
  return {
    common: { ...commonTokens }
  };
});
const isRouterReady = ref(false);
const router = useRouter();
router.isReady().finally(() => isRouterReady.value = true);
</script>

<template>
  <NConfigProvider :theme="tokenTheme" :theme-overrides="themeOverrides" class="h-full">
    <NNotificationProvider>
      <NMessageProvider>
        <div v-if="!isRouterReady" class="flex justify-center items-center h-screen">
          <NSpin size="large" />
        </div>
        <RouterView v-else />
      </NMessageProvider>
    </NNotificationProvider>
  </NConfigProvider>
</template>
<style>
.app-loader {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
}
</style>
