<script setup lang="ts">
import { ref } from 'vue';
import { useAuthStore } from '@/stores/auth';
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from '@/components/ui/card';
import { Label } from '@/components/ui/label';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import ThemePopover from '@/components/ThemePopover.vue';
import {
  NButton,
  NInput,
} from 'naive-ui';

const username = ref('');
const password = ref('');
const { login } = useAuthStore();

async function submitLogin() {
  await login(username.value, password.value);
}
</script>

<template>
  <form class="flex justify-center items-center h-screen">
    <Card class="w-full max-w-sm">
      <CardHeader>
        <CardTitle class="text-2xl">
          Login
        </CardTitle>
      </CardHeader>
      <CardContent class="grid gap-4">
        <div class="grid gap-2">
          <Label for="username">Username</Label>
          <Input v-model="username" id="username" type="text" placeholder="m@example.com" required />
        </div>
        <div class="grid gap-2">
          <Label for="password">Password</Label>
          <Input v-model="password" id="password" type="password" required />
        </div>
      </CardContent>
      <CardFooter>
        <Button class="w-full mr-1" @click.prevent="submitLogin">
          Sign in
        </Button>
        <ThemePopover />
      </CardFooter>
      <NButton type="primary">
        Primary
      </NButton>
      <NInput type="text" placeholder="Basic Input" />
    </Card>
  </form>
</template>
