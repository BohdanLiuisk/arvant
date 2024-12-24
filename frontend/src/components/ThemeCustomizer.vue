<script setup lang="ts">
import type { Color } from '@/types/color';
import colors from 'tailwindcss/colors';
import { useAppearanceStore, RADIUS } from '@/stores/appearance';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { CheckIcon, MoonIcon, SunIcon } from 'lucide-vue-next';

defineProps<{
  allColors: Color[]
}>();

const { theme, radius, setRadius, setTheme, isDark } = useAppearanceStore();
</script>

<template>
  <div class="p-4">
    <div class="grid space-y-1">
      <h1 class="text-md text-foreground font-semibold">
        Customize
      </h1>
      <p class="text-xs text-muted-foreground">
        Pick a style and color for your components.
      </p>
    </div>
    <div class="space-y-1.5 pt-6">
      <Label for="color" class="text-xs"> Color </Label>
      <div class="grid grid-cols-3 gap-2 py-1.5">
        <Button
          v-for="(color, index) in allColors"
          :key="index"
          variant="outline"
          class="h-8 justify-start px-3"
          :class="color === theme ? 'border-foreground border-2': ''"
          @click="setTheme(color)"
        >
          <span
            class="h-5 w-5 rounded-full flex items-center justify-center shrink-0"
            :style="{ backgroundColor: colors[color]['600'] }"
          >
            <CheckIcon v-if="color === theme" class="h-3 w-3 text-white" />
          </span>
          <span class="ml-2 text-xs capitalize">
            {{ color }}
          </span>
        </Button>
      </div>
    </div>
    <div class="space-y-1.5 pt-6">
      <Label for="radius" class="text-xs"> Radius </Label>
      <div class="grid grid-cols-5 gap-2 py-1.5">
        <Button
          v-for="(r, index) in RADIUS"
          :key="index"
          variant="outline"
          class="h-8 justify-center px-3"
          :class="r === radius ? 'border-foreground border-2' : ''"
          @click="setRadius(r)"
        >
          <span class="text-xs">
            {{ r }}
          </span>
        </Button>
      </div>
    </div>
    <div class="space-y-1.5 pt-6">
      <Label for="theme" class="text-xs"> Theme </Label>
      <div class="flex space-x-2 py-1.5">
        <Button
          class="h-8"
          variant="outline"
          :class="{ 'border-2 border-foreground': !isDark }"
          @click="isDark = false"
        >
          <SunIcon class="w-4 h-4 mr-2" />
          <span class="text-xs">Light</span>
        </Button>
        <Button
          class="h-8"
          variant="outline"
          :class="{ 'border-2 border-foreground': isDark }"
          @click="isDark = true"
        >
          <MoonIcon class="w-4 h-4 mr-2" />
          <span class="text-xs">Dark</span>
        </Button>
      </div>
    </div>
  </div>
</template>
