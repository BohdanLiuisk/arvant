<script setup lang="ts">
import ArvantLogo from '@/components/ArvantLogo.vue';
import {
  SidebarHeader,
  SidebarSeparator,
  SidebarTrigger,
  useSidebar
} from '@/components/ui/sidebar';
const { isMobile } = useSidebar();
</script>

<template>
  <SidebarHeader>
    <div class="flex items-center justify-between group-data-[collapsible=icon]:justify-center">
      <a href="/" class="flex flex-row gap-2 items-center justify-center group-data-[collapsible=icon]:hidden ml-2">
        <ArvantLogo class="size-8" />
        <div class="grid flex-1 text-left text-lg leading-tight">
          <span class="truncate">Arvant</span>
        </div>
      </a>
      <SidebarTrigger v-if="!isMobile" />
    </div>
  </SidebarHeader>
  <SidebarSeparator />
</template>
