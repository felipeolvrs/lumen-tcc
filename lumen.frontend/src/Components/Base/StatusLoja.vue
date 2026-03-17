<script setup lang="ts">
// Recebe e sincroniza o status direto com o objeto profile do pai
const isStoreActive = defineModel<boolean>()
</script>

<template>
  <div
      class="bg-white p-5 rounded-3xl shadow-soft border border-slate-100 flex items-center justify-between transition-colors"
      :class="isStoreActive ? 'border-emerald-500' : 'border-slate-200'"
  >
    <div>
      <div class="flex items-center gap-2 mb-1">
        <div
            class="w-2 h-2 rounded-full"
            :class="isStoreActive ? 'bg-emerald-500 animate-pulse' : 'bg-slate-300'"
        ></div>
        <h3 class="font-bold text-slate-900 text-sm">Loja {{isStoreActive ? 'Online': 'Offline'}}</h3>
      </div>
      <p class="text-xs text-slate-500 font-medium">
        {{isStoreActive ? 'Sua vitrine está ativa' : 'Sua vitrine está oculta'}}
      </p>
    </div>

    <label class="relative inline-flex items-center cursor-pointer">
      <input
          type="checkbox"
          v-model="isStoreActive"
          class="peer sr-only"
      />
      <div
          class="w-14 h-8 bg-slate-200 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[4px] after:left-[4px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-6 after:w-6 after:transition-all peer-checked:bg-slate-900"
      ></div>
    </label>
  </div>
</template>