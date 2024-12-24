<script setup lang="ts">
import { Separator } from '@/components/ui/separator';
import {
  Breadcrumb,
  BreadcrumbItem, BreadcrumbLink,
  BreadcrumbList,
  BreadcrumbPage,
  BreadcrumbSeparator
} from '@/components/ui/breadcrumb';
import { RouterView } from 'vue-router';
import { SidebarTrigger, useSidebar } from '@/components/ui/sidebar';
import {
  DropdownMenu,
  DropdownMenuTrigger,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuItem,
  DropdownMenuPortal,
  DropdownMenuSub,
  DropdownMenuSubTrigger,
  DropdownMenuSubContent,
  DropdownMenuCheckboxItem
} from '@/components/ui/dropdown-menu';
import { Avatar, AvatarImage, AvatarFallback } from '@/components/ui/avatar';
import { BellIcon, LogOutIcon, LanguagesIcon } from 'lucide-vue-next';
import ThemePopover from '@/components/ThemePopover.vue';
import { useLocalStorage } from '@vueuse/core';
import { AVAILABLE_LOCALES } from '@/plugins/i18';
import { useI18n } from 'vue-i18n';

const { isMobile, setOpenMobile } = useSidebar();
const { t, locale } = useI18n();
const currentLocale = useLocalStorage<string>('arvant-locale', 'en');
const user = {
  name: 'Bohdan Liusik',
  email: 'liusikbogdan@gmail.com',
  avatar: 'https://i.gyazo.com/89238eb9fa6420a970c040f9c28f4d99.jpg'
};
</script>

<template>
  <header class=" sticky top-0 z-30 flex h-14 shrink-0
    items-center gap-2 border-b bg-background px-4"
  >
    <div class="flex items-center">
      <SidebarTrigger v-if="isMobile" class="-ml-1" @click="setOpenMobile(true)" />
      <Separator v-if="isMobile" orientation="vertical" class="mr-2 h-4" />
      <Breadcrumb>
        <BreadcrumbList>
          <BreadcrumbItem class="hidden md:block">
            <BreadcrumbLink href="#"> Building Your Application </BreadcrumbLink>
          </BreadcrumbItem>
          <BreadcrumbSeparator class="hidden md:block" />
          <BreadcrumbItem>
            <BreadcrumbPage>Data Fetching</BreadcrumbPage>
          </BreadcrumbItem>
          <BreadcrumbSeparator class="hidden md:block" />
          <BreadcrumbItem class="hidden md:block">
            <BreadcrumbPage>test</BreadcrumbPage>
          </BreadcrumbItem>
        </BreadcrumbList>
      </Breadcrumb>
    </div>
    <div class="flex items-center justify-end ml-auto flex-1 md:grow-0">
      <ThemePopover class="mr-2 rounded-full"/>
      <DropdownMenu :modal="false">
        <DropdownMenuTrigger as-child>
          <Button variant="ghost" class="relative h-9 w-9">
            <Avatar class="h-9 w-9 rounded-md">
              <AvatarImage :src="user.avatar" :alt="user.name" />
              <AvatarFallback>SC</AvatarFallback>
            </Avatar>
          </Button>
        </DropdownMenuTrigger>
        <DropdownMenuContent
          class="w-[--radix-dropdown-menu-trigger-width] min-w-56 rounded-lg"
          align="end"
          :side-offset="4"
        >
          <DropdownMenuLabel class="p-0 font-normal">
            <div class="flex items-center gap-2 px-1 py-1.5 text-left text-sm">
              <Avatar class="h-8 w-8 rounded-lg">
                <AvatarImage :src="user.avatar" :alt="user.name" />
                <AvatarFallback class="rounded-lg">
                  CN
                </AvatarFallback>
              </Avatar>
              <div class="grid flex-1 text-left text-sm leading-tight">
                <span class="truncate font-semibold">{{ user.name }}</span>
                <span class="truncate text-xs">{{ user.email }}</span>
              </div>
            </div>
          </DropdownMenuLabel>
          <DropdownMenuSeparator />
          <DropdownMenuGroup>
            <DropdownMenuSub>
              <DropdownMenuSubTrigger>
                <LanguagesIcon class="mr-2 h-4 w-4" />
                {{ t('header.lang') }}
              </DropdownMenuSubTrigger>
              <DropdownMenuPortal>
                <DropdownMenuSubContent>
                  <DropdownMenuItem
                    v-for="lang of AVAILABLE_LOCALES"
                    :key="lang.code"
                    @select="() => {
                      locale = lang.code
                      currentLocale = lang.code
                    }"
                  >
                    <DropdownMenuCheckboxItem :checked="currentLocale === lang.code" />
                    {{ lang.name }}
                  </DropdownMenuItem>
                </DropdownMenuSubContent>
              </DropdownMenuPortal>
            </DropdownMenuSub>
            <DropdownMenuItem>
              <BellIcon />
              Notifications
            </DropdownMenuItem>
          </DropdownMenuGroup>
          <DropdownMenuSeparator />
          <DropdownMenuItem>
            <LogOutIcon />
            Log out
          </DropdownMenuItem>
        </DropdownMenuContent>
      </DropdownMenu>
    </div>
  </header>
  <div class="flex flex-1 flex-col gap-4 p-4">
    <RouterView />
  </div>
</template>
