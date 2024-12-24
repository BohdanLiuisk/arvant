import './assets/font.css';
import './assets/index.css';
import './assets/themes.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from '@/plugins/router';
import { i18n } from '@/plugins/i18';

const app = createApp(App);
app
  .use(createPinia())
  .use(i18n)
  .use(router);
app.mount('#app');
