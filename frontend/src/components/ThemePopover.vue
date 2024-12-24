<script setup lang="ts">
import { Button } from '@/components/ui/button';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { useAppearanceStore } from '@/stores/appearance';
import { PaintbrushIcon } from 'lucide-vue-next';
import { onMounted, useAttrs, watch } from 'vue';
import ThemeCustomizer from './ThemeCustomizer.vue';
import { allColors } from '@/constants/allColors';

const { theme, radius } = useAppearanceStore();
const triggerAttrs = useAttrs();

onMounted(() => {
  document.documentElement.style.setProperty('--radius', `${radius.value}rem`);
  document.documentElement.classList.add(`theme-${theme.value}`);
});

watch(theme, (theme) => {
  document.documentElement.classList.remove(
    ...allColors.map(color => `theme-${color}`)
  );
  document.documentElement.classList.add(`theme-${theme}`);
});

watch(radius, (radius) => {
  document.documentElement.style.setProperty('--radius', `${radius}rem`);
});
</script>

<template>
  <Popover>
    <PopoverTrigger as-child>
      <Button class="w-9 h-9" v-bind="triggerAttrs" :variant="'default'" :size="'default'">
        <PaintbrushIcon class="w-4 h-4" />
      </Button>
    </PopoverTrigger>
    <PopoverContent :side-offset="8" align="end" class="w-96">
      <ThemeCustomizer :all-colors="allColors" />
    </PopoverContent>
  </Popover>
</template>
