<script setup lang="ts">
import { ref } from 'vue';
import { type Produto } from '../Services/ProdutoService';
import { useCartStore } from '../Store/useCartStore';
import router from "../Router";

const props = defineProps<{
  produto: Produto | null;
}>();

const emit = defineEmits<{
  'open-ver-detalhes': [];
  'close-drawer': [];
}>();

const cart = useCartStore();
const quantity = ref(1);

const formatPrice = (value: number) => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(value);
};

const addToCart = () => {
  if (props.produto) {
    for (let i = 0; i < quantity.value; i++) {
      cart.addItem(props.produto);
    }
    emit('close-drawer');
    setTimeout(() => { quantity.value = 1; }, 300);
  }
};

const shareProduct = () => {
  if (navigator.share && props.produto) {
    navigator.share({
      title: props.produto.nome,
      text: props.produto.descricao,
      url: window.location.href,
    });
  }
};

const handleEditProduto = () => {
  if (props.produto != null)
    router.push(`/produto/editar/${props.produto.id}`);
  emit('close-drawer');
}
</script>

<template>
  <div v-if="produto" class="relative bg-white rounded-t-[32px] overflow-hidden pt-4 pb-2 "  @click="emit('close-drawer')">

    <button
        @click="emit('close-drawer')"
        class="absolute right-5 top-5 p-2 bg-slate-100 text-slate-500 rounded-full active:scale-90 transition-all"
    >
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="currentColor" class="w-5 h-5">
        <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
      </svg>
    </button>

    <button
        @click="shareProduct"
        class="absolute right-16 top-5 p-2 bg-slate-100 text-indigo-600 rounded-full active:scale-90 transition-all"
    >
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-5 h-5">
        <path stroke-linecap="round" stroke-linejoin="round" d="M3 16.5v2.25A2.25 2.25 0 005.25 21h13.5A2.25 2.25 0 0021 18.75V16.5m-13.5-9L12 3m0 0l4.5 4.5M12 3v13.5" />
      </svg>
    </button>

    <button
        @click.stop="handleEditProduto"
        class="absolute right-[108px] top-5 p-2 bg-slate-100 text-indigo-600 rounded-full active:scale-90 transition-all z-10"
    >
      <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil" viewBox="0 0 16 16">
        <path d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325"/>
      </svg>
    </button>

    <div class="p-6 pt-2 pb-10">
      <div class="flex gap-5 mb-8">
        <div class="w-28 h-28 bg-slate-100 rounded-2xl flex-shrink-0 overflow-hidden shadow-sm border border-slate-100">
          <img :src="produto.urlImagem" class="w-full h-full object-cover" />
        </div>
        <div class="flex-1 py-1">
          <h2 class="text-lg font-black text-slate-900 leading-snug mb-2 line-clamp-2">
            {{ produto.nome }}
          </h2>
          <div class="flex flex-col">
            <p class="text-2xl font-black text-slate-900 tracking-tight">
              {{ formatPrice(produto.preco) }}
            </p>
          </div>
        </div>
      </div>

      <div class="space-y-3">

        <div class="flex gap-3 h-14">
          <div class="flex items-center bg-slate-50 border border-slate-200 rounded-2xl px-1">
            <button @click.stop="quantity > 1 ? quantity-- : null" class="w-10 h-full flex items-center justify-center text-slate-500 hover:text-slate-900 active:scale-90 transition-all">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="currentColor" class="w-4 h-4"><path stroke-linecap="round" stroke-linejoin="round" d="M19.5 12h-15" /></svg>
            </button>
            <span class="w-6 text-center text-sm font-black text-slate-900">{{ quantity }}</span>
            <button @click.stop="quantity++" class="w-10 h-full flex items-center justify-center text-slate-500 hover:text-slate-900 active:scale-90 transition-all">
              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="currentColor" class="w-4 h-4"><path stroke-linecap="round" stroke-linejoin="round" d="M12 4.5v15m7.5-7.5h-15" /></svg>
            </button>
          </div>

          <button
              @click.stop="addToCart"
              class="flex-1 bg-slate-900 text-white font-black text-sm rounded-2xl shadow-xl shadow-slate-900/20 active:scale-[0.98] transition-all flex items-center justify-center gap-2"
          >
            Adicionar
            <span v-if="quantity > 1" class="text-[10px] opacity-70 ml-1">({{ quantity }}x)</span>
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="w-5 h-5 ml-1">
              <path fill-rule="evenodd" d="M7.5 6v.75H5.513c-.96 0-1.764.724-1.865 1.679l-1.263 12A1.875 1.875 0 004.25 22.5h15.5a1.875 1.875 0 001.865-2.071l-1.263-12a1.875 1.875 0 00-1.865-1.679H16.5V6a4.5 4.5 0 10-9 0zM12 3a3 3 0 00-3 3v.75h6V6a3 3 0 00-3-3zm-3 8.25a3 3 0 106 0v-.75a.75.75 0 011.5 0v.75a4.5 4.5 0 11-9 0v-.75a.75.75 0 011.5 0v.75z" clip-rule="evenodd" />
            </svg>
          </button>
        </div>

        <button
            @click.stop="emit('open-ver-detalhes')"
            class="w-full bg-white border border-slate-200 text-slate-700 font-black text-sm py-4 rounded-2xl active:scale-[0.98] transition-all flex items-center justify-center gap-2"
        >
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="currentColor" class="w-5 h-5 text-indigo-600">
            <path stroke-linecap="round" stroke-linejoin="round" d="M19.5 14.25v-2.625a3.375 3.375 0 00-3.375-3.375h-1.5A1.125 1.125 0 0113.5 7.125v-1.5a3.375 3.375 0 00-3.375-3.375H8.25m0 12.75h7.5m-7.5 3H12M10.5 2.25H5.625c-.621 0-1.125.504-1.125 1.125v17.25c0 .621.504 1.125 1.125 1.125h12.75c.621 0 1.125-.504 1.125-1.125V11.25a9 9 0 00-9-9z" />
          </svg>
          Ver Detalhes & Review
        </button>
      </div>
    </div>
  </div>

  <div v-else class="p-12 text-center">
    <div class="w-16 h-16 bg-slate-50 rounded-full flex items-center justify-center mx-auto mb-4">
      <div class="w-8 h-8 bg-slate-200 rounded-full animate-pulse"></div>
    </div>
    <p class="text-sm font-bold text-slate-400">Carregando informações...</p>
  </div>
</template>