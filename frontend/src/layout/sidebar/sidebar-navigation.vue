<script setup lang="ts">
import {
  CalendarIcon,
  NewspaperIcon,
  PhoneIcon,
  Settings2Icon,
  UsersIcon,
  ChevronRightIcon
} from 'lucide-vue-next';
import { useI18n } from 'vue-i18n';
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import {
  SidebarGroup,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarMenuSub,
  SidebarMenuSubItem,
  useSidebar
} from '@/components/ui/sidebar';
import { Collapsible, CollapsibleTrigger, CollapsibleContent } from '@/components/ui/collapsible';

const { t } = useI18n();
const currentRoute = useRoute();
const sidebar = useSidebar();

const links = computed(() => {
  return [
    {
      name: t('sidebar.feed'),
      icon: NewspaperIcon,
      disabled: false,
      path: '/feed'
    },
    {
      name: t('sidebar.calendar'),
      icon: CalendarIcon,
      disabled: false,
      path: '/calendar'
    },
    {
      name: t('sidebar.friends'),
      icon: UsersIcon,
      disabled: false,
      path: '/friends'
    },
    {
      name: t('sidebar.calls'),
      icon: PhoneIcon,
      disabled: false,
      path: '/calls'
    },
    {
      name: t('sidebar.settings.label'),
      icon: Settings2Icon,
      disabled: false,
      path: '/settings',
      child: [
        {
          name: t('sidebar.settings.general'),
          path: '/settings/general'
        },
        {
          name: t('sidebar.settings.team'),
          path: '/settings/team'
        },
        {
          name: t('sidebar.settings.limits'),
          path: '/settings/limits'
        }
      ]
    }
  ];
});

function goToRoute() {
  if (sidebar.isMobile.value) {
    sidebar.setOpenMobile(false);
  }
}
</script>

<template>
  <SidebarGroup>
<!--    <SidebarGroupLabel>Platform</SidebarGroupLabel>-->
    <SidebarMenu>
      <SidebarMenuItem  v-for="item in links" :key="item.name">
        <SidebarMenuButton
          v-if="!item.child"
          as-child
          :tooltip="item.name"
          :variant="currentRoute.path === item.path ? 'active' : 'default'"
          @click="goToRoute"
        >
          <RouterLink :to="item.path!">
            <component :is="item.icon" />
            <span>{{ item.name }}</span>
          </RouterLink>
        </SidebarMenuButton>
        <Collapsible
          v-else
          as-child
          :default-open="false"
          class="group/collapsible"
        >
          <SidebarMenuItem>
            <CollapsibleTrigger as-child>
              <SidebarMenuButton
                :tooltip="item.name"
                :variant="currentRoute.path.startsWith(item.path) ? 'active' : 'default'"
              >
                <component :is="item.icon" />
                <span>{{ item.name }}</span>
                <ChevronRightIcon class="
                  ml-auto transition-transform duration-200
                  group-data-[state=open]/collapsible:rotate-90" />
              </SidebarMenuButton>
            </CollapsibleTrigger>
            <CollapsibleContent>
              <SidebarMenuSub>
                <SidebarMenuSubItem v-for="child in item.child" :key="child.name">
                  <SidebarMenuButton as-child @click="goToRoute">
                    <RouterLink :to="child.path!">
                      <span>{{ child.name }}</span>
                    </RouterLink>
                  </SidebarMenuButton>
                </SidebarMenuSubItem>
              </SidebarMenuSub>
            </CollapsibleContent>
          </SidebarMenuItem>
        </Collapsible>
      </SidebarMenuItem>
    </SidebarMenu>
  </SidebarGroup>
</template>
