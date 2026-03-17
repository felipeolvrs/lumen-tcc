<script setup lang="ts">
// Interfaces separadas para melhor leitura
interface HeaderTag {
  text: string;
  classes: string;
}

interface HeaderProfileProps {
  name: string;
  handle: string;
  avatar: string;
  bio?: string;
  isVerified?: boolean;
  isOnline?: boolean;
}

const props = withDefaults(defineProps<HeaderProfileProps>(), {
  bio: '',
  isVerified: false,
  isOnline: false,
});
</script>

<template>
  <header class="pt-5 pl-5 pb-5 bg-white border-b border-slate-100">

    <div class="flex justify-between items-start mb-4">
      <div class="relative">
        <div class="w-20 h-20 rounded-full p-1 ring-2 ring-indigo-100 bg-white">
          <img
              :src="props.avatar"
              :alt="props.name"
              class="w-full h-full object-cover rounded-full"
          />
        </div>
      </div>
    </div>

    <div class="mb-1">
      <div class="flex items-center gap-1.5 mb-1">
        <h1 class="text-xl font-extrabold text-slate-900 tracking-tight">
          {{ props.name }}
        </h1>
      </div>

      <p class="text-slate-500 text-sm font-medium">
        {{ props.handle }}
      </p>
    </div>

    <div
        class="text-sm text-slate-600 leading-relaxed mb-1 pr-5"
        v-html="props.bio"
    ></div>
  </header>
</template>

<style scoped>
</style>