import { reactive, watch } from 'vue';
import { useAppearanceStore } from '@/stores/appearance';
import { addAlphaToHex, convertHslToHex, getNextColorInPalette } from '@/lib/utils';

export function useNaiveDesignTokens() {
  const { theme, isDark, radius, themeVars } = useAppearanceStore();
  const commonTokens = reactive({
    primaryColor: '',
    primaryColorHover: '',
    primaryColorPressed: '',
    primaryColorSuppl: '',
    textColorBase: '',
    baseColor: '',
    dividerColor: '',
    borderColor: '',
    modalColor: '',
    popoverColor: '',
    tableColor: '',
    cardColor: '',
    bodyColor: '',
    borderRadius: ''
  });

  watch(
    [theme, isDark, radius],
    () => {
      const primaryColor = convertHslToHex(themeVars.value?.primary);
      commonTokens.primaryColor = primaryColor;
      commonTokens.primaryColorHover = addAlphaToHex(primaryColor, 0.9);
      commonTokens.primaryColorSuppl = addAlphaToHex(primaryColor, 0.9);
      commonTokens.primaryColorPressed = getNextColorInPalette(theme.value, primaryColor);
      commonTokens.textColorBase = convertHslToHex(themeVars.value?.foreground);
      commonTokens.baseColor = convertHslToHex(themeVars.value?.['primary-foreground']);
      commonTokens.dividerColor = convertHslToHex(themeVars.value?.border);
      commonTokens.borderColor = convertHslToHex(themeVars.value?.border);
      commonTokens.modalColor = convertHslToHex(themeVars.value?.popover);
      commonTokens.popoverColor = convertHslToHex(themeVars.value?.popover);
      commonTokens.tableColor = convertHslToHex(themeVars.value?.card);
      commonTokens.cardColor = convertHslToHex(themeVars.value?.card);
      commonTokens.bodyColor = convertHslToHex(themeVars.value?.background);
      commonTokens.borderRadius = `${radius.value.toString()}rem`;
    },
    { immediate: true }
  );

  return {
    commonTokens
  };
}
