<script setup lang="ts">
import SidebarFooter from './sidebar-footer.vue';
import SidebarHeader from './sidebar-header.vue';
import SidebarNavigation from './sidebar-navigation.vue';
import {
  Sidebar,
  SidebarContent,
  SidebarInset,
  SidebarProvider,
  SidebarSeparator
} from '@/components/ui/sidebar';
</script>

<template>
  <SidebarProvider>
    <Sidebar collapsible="icon">
      <SidebarHeader />
      <SidebarContent>
        <SidebarNavigation />
      </SidebarContent>
      <SidebarSeparator />
      <SidebarFooter />
    </Sidebar>
    <SidebarInset>
      <slot />
    </SidebarInset>
  </SidebarProvider>
</template>
