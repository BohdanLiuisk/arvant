import { useLocalStorage } from '@vueuse/core';
import { isRef, nextTick, watch } from 'vue';
import { createI18n } from 'vue-i18n';
import type { I18n, Locale } from 'vue-i18n';

export function setI18nLanguage(i18n: I18n, locale: Locale): void {
  if (i18n.mode !== 'legacy' && isRef(i18n.global.locale)) {
    i18n.global.locale.value = locale;
  } else {
    i18n.global.locale = locale;
  }
  document.querySelector('html')?.setAttribute('lang', locale);
}

export async function loadLocaleMessages(i18n: I18n, locale: Locale) {
  const messages = await import(`../locales/${locale}.json`).then(r => r.default || r);
  i18n.global.setLocaleMessage(locale, messages);
  return nextTick();
}

const locale = useLocalStorage('arvant-locale', 'en');

watch(locale, async (newLocale) => {
  await loadLocaleMessages(i18n, newLocale);
});

export const AVAILABLE_LOCALES = [
  {
    code: 'en',
    name: 'English'
  },
  {
    code: 'ua',
    name: 'Українська'
  }
];

function setupI18n(): I18n {
  const i18n = createI18n({
    locale: locale.value,
    availableLocales: AVAILABLE_LOCALES.map((locale) => locale.code),
    fallbackLocale: 'en',
    legacy: false
  }) as I18n;
  setI18nLanguage(i18n, locale.value);
  loadLocaleMessages(i18n, 'en');
  if (locale.value !== 'en') {
    loadLocaleMessages(i18n, locale.value);
  }
  return i18n;
}

export const i18n = setupI18n();
