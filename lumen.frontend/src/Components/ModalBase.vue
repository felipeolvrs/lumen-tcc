<script setup lang="ts">
// Definindo as propriedades do modal
const props = defineProps<{
  isOpen: boolean;
  title: string;
  message: string;
  type?: 'success' | 'error' | 'info'; // Diferencia visualmente a mensagem
}>();

defineEmits(['close']);

// Lógica de cores baseada no tipo
const getIconClass = () => {
  if (props.type === 'success') return 'ph-check-circle text-emerald-500';
  if (props.type === 'error') return 'ph-warning-circle text-rose-500';
  return 'ph-info text-blue-500';
};
</script>

<template>
  <div v-if="isOpen" class="fixed inset-0 z-[100] flex items-center justify-center p-6 bg-slate-900/60 backdrop-blur-sm">
    <div class="bg-white w-full max-w-xs rounded-[32px] p-8 shadow-2xl scale-in text-center">

      <div class="mb-4 flex justify-center">
        <i :class="['ph-fill text-6xl', getIconClass()]"></i>
      </div>

      <h3 class="text-xl font-black text-slate-900 mb-2">{{ title }}</h3>
      <p class="text-slate-500 text-sm mb-8 leading-relaxed">{{ message }}</p>

      <button
          @click="$emit('close')"
          class="w-full bg-slate-900 text-white py-4 rounded-2xl font-bold active:scale-95 transition-all shadow-lg shadow-slate-200"
      >
        Entendido
      </button>
    </div>
  </div>
</template>

<style scoped>
.scale-in {
  animation: scaleIn 0.25s cubic-bezier(0.175, 0.885, 0.32, 1.275);
}
@keyframes scaleIn {
  from { opacity: 0; transform: scale(0.8); }
  to { opacity: 1; transform: scale(1); }
}
</style>