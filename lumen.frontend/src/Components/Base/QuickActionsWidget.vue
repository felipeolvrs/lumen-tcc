<script setup lang="ts">
defineProps<{
  modeloLoja?: string
}>()

defineEmits(['open-business', 'open-metrics'])
</script>

<template>
  <div>
    <h3 class="text-xs font-bold text-slate-400 uppercase tracking-wider mb-3 px-1">
      Gestão Rápida
    </h3>
    <div class="grid grid-cols-2 gap-3">
      <button
          @click="$emit('open-business')"
          class="bg-white p-4 rounded-2xl shadow-soft border border-slate-100 flex flex-col items-start gap-3 hover:border-indigo-200 transition active:scale-95 text-left"
      >
        <div class="w-10 h-10 rounded-xl bg-indigo-50 text-indigo-600 flex items-center justify-center">
          <i class="ph-fill ph-storefront text-xl"></i>
        </div>
        <div>
          <span class="block text-sm font-bold text-slate-900">Modelo</span>
          <span class="text-[10px] text-indigo-600 font-black bg-indigo-50 px-2 py-0.5 rounded mt-1 inline-block uppercase">
            {{ modeloLoja || 'Híbrido' }}
          </span>
        </div>
      </button>

      <button
          @click="$emit('open-metrics')"
          class="bg-white p-4 rounded-2xl shadow-soft border border-slate-100 flex flex-col items-start gap-3 hover:border-purple-200 transition active:scale-95 text-left"
      >
        <div class="w-10 h-10 rounded-xl bg-purple-50 text-purple-600 flex items-center justify-center">
          <i class="ph-fill ph-chart-line-up text-xl"></i>
        </div>
        <div>
          <span class="block text-sm font-bold text-slate-900">Métricas</span>
          <span class="text-xs text-slate-500 font-medium mt-1 inline-block">Visualizar</span>
        </div>
      </button>
    </div>
  </div>
</template>