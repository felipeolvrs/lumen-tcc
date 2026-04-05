<script setup lang="ts">
interface ConfigHeaderProps {
  title: string
  showTitle?: boolean
  showBack?: boolean
  showShare?: boolean
}

withDefaults(defineProps<ConfigHeaderProps>(), {
  showTitle: true, // Mudei para true para o título aparecer
  showBack: true,
  showShare: false // Mudei para false para sumir conforme seu pedido
})

defineEmits(['share'])
</script>

<template>
  <div
      class="sticky top-0 bg-white/80 backdrop-blur-md z-20 flex items-center justify-between p-4 border-b border-slate-100"
  >
    <div class="w-10 flex justify-start">
      <button
          v-if="showBack"
          @click="$router.back()"
          class="p-2 -ml-2 text-slate-600 hover:bg-slate-50 rounded-full transition"
      >
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-6 h-6">
          <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 19.5L8.25 12l7.5-7.5" />
        </svg>
      </button>
    </div>

    <span
        class="text-sm font-bold text-slate-900 transition-opacity duration-300 flex-1 text-center truncate px-2"
        :class="showTitle ? 'opacity-100' : 'opacity-0'"
    >
      {{ title }}
    </span>

    <div class="w-10 flex justify-end">
      <div v-if="showShare">
        <slot name="action">
          <button
              @click="$emit('share')"
              class="p-2 -mr-2 text-slate-600 hover:bg-slate-50 rounded-full transition"
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-6 h-6">
              <path stroke-linecap="round" stroke-linejoin="round" d="M3 16.5v2.25A2.25 2.25 0 005.25 21h13.5A2.25 2.25 0 0021 18.75V16.5m-13.5-9L12 3m0 0l4.5 4.5M12 3v13.5" />
            </svg>
          </button>
        </slot>
      </div>
    </div>
  </div>
</template>