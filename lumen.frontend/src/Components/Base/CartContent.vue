<script setup lang="ts">
import { ref } from 'vue';
import { useCartStore } from '../../Store/useCartStore';

const emit = defineEmits<{
  'go-to-checkout': [];
  'close-drawer': [];
}>();

const cart = useCartStore();
const isConfirmingClear = ref(false);

const DEFAULT_IMAGE = "https://placehold.co/400x400/f1f5f9/94a3b8?text=Sem+Foto";

const formatPrice = (value: number) => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(value);
};

const handleClearCart = () => {
  cart.clearCart();
  isConfirmingClear.value = false;
};
</script>

<template>
  <div class="flex flex-col h-full bg-white relative overflow-hidden">

    <div class="px-6 py-5 border-b border-slate-100 flex items-center justify-between flex-shrink-0 relative z-20 bg-white">
      <h2 class="text-xl font-black text-slate-900 tracking-tight">Seu Carrinho</h2>
      <button
          v-if="cart.items.value.length > 0 && !isConfirmingClear"
          @click="isConfirmingClear = true"
          class="text-[10px] font-black uppercase tracking-widest text-slate-400 hover:text-rose-500 transition-colors"
      >
        Limpar
      </button>
    </div>

    <Transition name="slide-down">
      <div v-if="isConfirmingClear" class="absolute top-[72px] left-0 right-0 z-10 bg-rose-50 border-b border-rose-100 px-6 py-6 flex flex-col gap-4 animate-in fade-in duration-300">
        <div class="flex items-center justify-between">
          <span class="text-xs font-black text-rose-600 uppercase tracking-widest">Esvaziar carrinho?</span>
          <div class="flex gap-4">
            <button @click="isConfirmingClear = false" class="text-xs font-black uppercase text-slate-400">Não</button>
            <button @click="handleClearCart" class="text-xs font-black uppercase text-rose-600 bg-white px-4 py-2 rounded-xl shadow-sm">Sim, limpar</button>
          </div>
        </div>
      </div>
    </Transition>

    <div
        v-if="cart.items.value.length === 0"
        class="flex-1 flex flex-col items-center justify-center p-8 text-center"
    >
      <div class="w-20 h-20 bg-slate-50 rounded-full flex items-center justify-center mb-6 text-slate-200">
        <i class="ph-fill ph-shopping-cart-simple text-4xl"></i>
      </div>
      <p class="text-slate-900 font-black mb-1">Vazio por aqui</p>
      <p class="text-slate-400 text-xs mb-8 max-w-[180px] leading-relaxed">
        Sua sacola está vazia. Adicione produtos para continuar.
      </p>
      <button
          @click="$emit('close-drawer')"
          class="px-8 py-3.5 bg-slate-900 text-white rounded-2xl text-xs font-black active:scale-95 transition-all shadow-xl shadow-slate-100"
      >
        Voltar à Vitrine
      </button>
    </div>

    <div v-else class="flex-1 overflow-y-auto p-6 space-y-4 pt-8">
      <div
          v-for="item in cart.items.value"
          :key="item.produto.id"
          class="flex items-center gap-4"
      >
        <div class="w-14 h-14 rounded-2xl overflow-hidden flex-shrink-0 bg-slate-50 border border-slate-50">
          <img
              :src="item.produto.urlImagem || DEFAULT_IMAGE"
              :alt="item.produto.nome"
              class="w-full h-full object-cover"
          />
        </div>

        <div class="flex-1 min-w-0">
          <p class="text-xs font-black text-slate-900 truncate">{{ item.produto.nome }}</p>
          <p class="text-[10px] font-bold text-slate-400 mt-0.5 uppercase tracking-wider">{{ formatPrice(item.produto.preco) }}</p>
        </div>

        <div class="flex items-center gap-2 bg-slate-50 p-1 rounded-xl border border-slate-100">
          <button
              @click="cart.updateQuantity(item.produto.id!, item.quantidade - 1)"
              class="w-7 h-7 rounded-lg bg-white flex items-center justify-center text-slate-900 shadow-sm border border-slate-50 active:scale-90 transition-all"
          >
            <i class="ph-bold ph-minus text-[8px]"></i>
          </button>
          <span class="text-[10px] font-black w-4 text-center text-slate-900">{{ item.quantidade }}</span>
          <button
              @click="cart.updateQuantity(item.produto.id!, item.quantidade + 1)"
              class="w-7 h-7 rounded-lg bg-slate-900 flex items-center justify-center text-white active:scale-90 transition-all shadow-md shadow-slate-200"
          >
            <i class="ph-bold ph-plus text-[8px]"></i>
          </button>
        </div>
      </div>
    </div>

    <div
        v-if="cart.items.value.length > 0"
        class="p-6 bg-white border-t border-slate-50 flex-shrink-0 pb-safe"
    >
      <div class="flex justify-between items-center mb-6">
        <span class="text-slate-400 text-[10px] font-black uppercase tracking-widest">Total Estimado</span>
        <span class="text-xl font-black text-slate-900">{{ formatPrice(cart.total.value) }}</span>
      </div>

      <button
          @click="emit('go-to-checkout')"
          class="w-full bg-slate-900 text-white font-bold text-sm py-4.5 rounded-2xl shadow-xl shadow-slate-100 active:scale-[0.98] transition-all flex items-center justify-center gap-3"
      >
        <span>Confirmar Pedido</span>
        <i class="ph-bold ph-arrow-right text-base"></i>
      </button>
    </div>
  </div>
</template>

<style scoped>
.pb-safe {
  padding-bottom: calc(1rem + env(safe-area-inset-bottom));
}

.slide-down-enter-active, .slide-down-leave-active {
  transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}
.slide-down-enter-from, .slide-down-leave-to {
  transform: translateY(-100%);
  opacity: 0;
}
</style>