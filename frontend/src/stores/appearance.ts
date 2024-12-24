import type { Theme } from '@/assets/themes';
import { themes } from '@/assets/themes';
import { useDark, useStorage } from '@vueuse/core';
import { computed } from 'vue';

interface AppearanceConfig {
  theme: Theme['name'];
  radius: number;
}

export const RADIUS = [0, 0.25, 0.5, 0.75, 1];

export function useAppearanceStore() {
  const isDark = useDark();
  const config = useStorage<AppearanceConfig>('appearanceConfig', {
    theme: 'zinc',
    radius: 0.5
  });

  const themeClass = computed(() => `theme-${config.value.theme}`);

  const theme = computed(() => config.value.theme);
  const radius = computed(() => config.value.radius);

  function setTheme(themeName: Theme['name']) {
    config.value.theme = themeName;
  }

  function setRadius(newRadius: number) {
    config.value.radius = newRadius;
  }

  const themeVars = computed(() => {
    const t = themes.find((t) => t.name === theme.value);
    return t?.cssVars[isDark.value ? 'dark' : 'light'];
  });

  return {
    isDark,
    config,
    theme,
    setTheme,
    radius,
    setRadius,
    themeClass,
    themeVars
  };
}
