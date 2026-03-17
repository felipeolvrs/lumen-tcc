<script setup lang="ts">
import { watch, onUnmounted } from 'vue';

const props = defineProps<{
  modal?: {
    show: boolean;
    title: string;
    message: string;
    type?: 'success' | 'error';
  }
}>();

const emit = defineEmits(['close-modal']);

watch(() => props.modal?.show, (isOpen) => {
  if (typeof document !== 'undefined') {
    if (isOpen) {
      document.body.style.overflow = 'hidden';
    } else {
      document.body.style.overflow = '';
    }
  }
});

onUnmounted(() => {
  if (typeof document !== 'undefined') {
    document.body.style.overflow = '';
  }
});
</script>

<template>
  <div class="bg-slate-50 text-slate-900 antialiased selection:bg-slate-200 min-h-screen flex justify-center">
    <main class="w-full max-w-md min-h-screen bg-white relative shadow-2xl overflow-hidden pb-[90px] flex flex-col">

      <div class="flex-1 overflow-y-auto">
        <slot />
      </div>

      <div class="absolute bottom-0 left-0 w-full z-10">
        <slot name="footer" />
      </div>

      <Transition name="fade">
        <div v-if="modal?.show" class="fixed inset-0 z-[100] flex items-center justify-center p-6 bg-slate-900/60 backdrop-blur-sm">
          <div class="bg-white w-full max-w-[300px] rounded-[32px] p-8 shadow-2xl text-center scale-in">

            <div class="mb-4">
              <i v-if="modal.type === 'success'" class="ph-fill ph-check-circle text-6xl text-emerald-500"></i>
              <i v-else class="ph-fill ph-warning-circle text-6xl text-rose-500"></i>
            </div>

            <h3 class="text-xl font-black text-slate-900 mb-2 leading-tight">{{ modal.title }}</h3>
            <p class="text-slate-500 text-sm mb-8 leading-relaxed">{{ modal.message }}</p>

            <button
                @click="emit('close-modal')"
                class="w-full bg-slate-900 text-white py-4 rounded-2xl font-bold active:scale-95 transition-all shadow-lg"
            >
              Entendido
            </button>
          </div>
        </div>
      </Transition>

    </main>
  </div>
</template>

<style scoped>
.scale-in {
  animation: scaleIn 0.25s cubic-bezier(0.175, 0.885, 0.32, 1.275);
}

@keyframes scaleIn {
  from { opacity: 0; transform: scale(0.85); }
  to { opacity: 1; transform: scale(1); }
}

.fade-enter-active, .fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
}
</style>