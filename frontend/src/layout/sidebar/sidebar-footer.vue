<script setup lang="ts">
import {
  SidebarFooter,
  SidebarMenu,
  SidebarMenuItem
} from '@/components/ui/sidebar';
import SidebarProfile from './sidebar-profile.vue';

</script>

<template>
  <SidebarFooter>
    <SidebarMenu>
      <SidebarMenuItem>
        <SidebarProfile />
      </SidebarMenuItem>
    </SidebarMenu>
  </SidebarFooter>
</template>
