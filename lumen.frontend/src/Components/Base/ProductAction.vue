<script setup lang="ts">
import { ref } from 'vue';
import { useCartStore } from "../../Store/useCartStore.ts";

const props = defineProps<{
  product: any
}>();

const cart = useCartStore();
const showToast = ref(false);
const isAnimating = ref(false);

const addToCart = () => {
  if (props.product) {
    cart.addItem(props.product);

    showToast.value = true;
    isAnimating.value = true;

    setTimeout(() => {
      showToast.value = false;
    }, 2000);

    setTimeout(() => {
      isAnimating.value = false;
    }, 300);
  }
};
</script>

<template>
  <div class="fixed bottom-0 left-0 right-0 z-50 max-w-md mx-auto pointer-events-none">

    <Transition name="slide-up">
      <div v-if="showToast" class="flex justify-center mb-4">
        <div class="bg-slate-900 text-white px-6 py-3 rounded-full shadow-2xl flex items-center gap-3 border border-slate-800 backdrop-blur-md">
          <i class="ph-fill ph-check-circle text-emerald-400"></i>
          <span class="text-xs font-black tracking-tight">Adicionado ao carrinho!</span>
        </div>
      </div>
    </Transition>

    <div class="p-4 bg-white/90 backdrop-blur-xl border-t border-slate-100 pointer-events-auto pb-safe">
      <div class="flex gap-3">
        <button
            @click="addToCart"
            class="flex-1 bg-slate-900 text-white font-black text-sm py-4 rounded-2xl shadow-xl transition-all flex items-center justify-center gap-2 active:scale-95 overflow-hidden relative"
        >
          <div :class="{'translate-y-0 opacity-100': !isAnimating, '-translate-y-10 opacity-0': isAnimating}" class="flex items-center gap-2 transition-all duration-300">
            <span>Adicionar</span>
            <i class="ph-bold ph-shopping-bag-open text-lg"></i>
          </div>

          <div :class="{'translate-y-0 opacity-100': isAnimating, 'translate-y-10 opacity-0': !isAnimating}" class="absolute flex items-center gap-2 transition-all duration-300 text-emerald-400">
            <i class="ph-bold ph-check text-lg"></i>
          </div>
        </button>

        <button
            class="p-4 border-2 border-slate-100 rounded-2xl text-slate-300 hover:text-rose-500 hover:bg-rose-50 hover:border-rose-100 transition-all active:scale-90"
        >
          <i class="ph-bold ph-heart text-xl"></i>
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.pb-safe {
  padding-bottom: calc(1rem + env(safe-area-inset-bottom));
}

.slide-up-enter-active, .slide-up-leave-active {
  transition: all 0.4s cubic-bezier(0.16, 1, 0.3, 1);
}
.slide-up-enter-from {
  transform: translateY(20px);
  opacity: 0;
}
.slide-up-leave-to {
  transform: translateY(-20px);
  opacity: 0;
}

.fade-enter-active, .fade-leave-active {
  transition: all 0.2s ease;
}
.fade-enter-from, .fade-leave-to {
  opacity: 0;
  transform: translateY(4px);
}
</style>