<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { type ColecaoResponse } from '../Services/ColecaoService';
import { useCartStore } from '../Store/useCartStore';
import type { Produto } from '../Services/ProdutoService';

const props = defineProps<{
  colecao: ColecaoResponse | null;
}>();

const emit = defineEmits<{
  'close-drawer': [];
  'ver-detalhes': [id: string];
}>();

const router = useRouter();
const cart = useCartStore();
const isSuccess = ref(false);

const DEFAULT_IMAGE = "https://placehold.co/400x400/f1f5f9/94a3b8?text=Sem+Foto";

const formatPrice = (value: number) => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(value);
};

const addAllToCart = () => {
  if (props.colecao?.produtos) {
    props.colecao.produtos.forEach((p: Produto) => cart.addItem(p));

    isSuccess.value = true;
    setTimeout(() => {
      emit('close-drawer');
      isSuccess.value = false;
    }, 1000);
  }
};

const handleEditColecao = () => {
  if (props.colecao?.id) {
    router.push({ name: 'colecao.editar', params: { id: props.colecao.id } });
    emit('close-drawer');
  }
};
</script>

<template>
  <div v-if="colecao" class="relative bg-white rounded-t-2xl overflow-hidden">
    <div class="absolute right-5 top-5 flex gap-2 z-20">
      <button
          @click.stop="handleEditColecao"
          class="p-2.5 bg-slate-100 text-indigo-600 rounded-full active:scale-90 transition-all shadow-sm"
          title="Editar Kit"
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil" viewBox="0 0 16 16">
          <path d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325"/>
        </svg>
      </button>

      <button
          @click="emit('close-drawer')"
          class="p-2.5 bg-slate-100 text-slate-500 rounded-full active:scale-90 transition-all shadow-sm"
      >
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="currentColor" class="w-5 h-5">
          <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>
    </div>

    <div class="p-6 pt-12 space-y-6">
      <div class="flex items-center gap-4 pr-24">
        <div class="w-20 h-20 rounded-2xl overflow-hidden bg-slate-100 flex-shrink-0 shadow-sm border border-slate-100">
          <img :src="colecao.urlCapa || DEFAULT_IMAGE" :alt="colecao.nome" class="w-full h-full object-cover" />
        </div>
        <div class="flex-1">
          <h3 class="text-xl font-black text-slate-900 leading-tight">{{ colecao.nome }}</h3>
          <p class="text-xs text-slate-400 font-bold uppercase tracking-wider mt-1">
            {{ colecao.produtos?.length || 0 }} produtos no kit
          </p>
        </div>
      </div>

      <div v-if="colecao.descricao" class="bg-slate-50 p-4 rounded-xl border-l-4 border-slate-900">
        <p class="text-sm text-slate-600 leading-relaxed">
          {{ colecao.descricao }}
        </p>
      </div>

      <div class="space-y-3">
        <label class="block text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1 mb-1">
          O que vem neste kit:
        </label>

        <div class="max-h-[350px] overflow-y-auto pr-1 space-y-3 custom-scrollbar">
          <div
              v-for="produto in colecao.produtos"
              :key="produto.id"
              @click="emit('ver-detalhes', produto.id!)"
              class="flex items-center gap-3 p-3 bg-white border border-slate-100 rounded-2xl shadow-sm cursor-pointer active:scale-[0.98] transition-all"
          >
            <div class="w-14 h-14 rounded-xl overflow-hidden flex-shrink-0 shadow-inner">
              <img :src="produto.urlImagem || DEFAULT_IMAGE" :alt="produto.nome" class="w-full h-full object-cover" />
            </div>
            <div class="flex-1 min-w-0">
              <p class="text-sm font-black text-slate-900 truncate">{{ produto.nome }}</p>
              <p class="text-xs font-bold text-slate-500">{{ formatPrice(produto.preco) }}</p>
            </div>
            <button
                class="p-2.5 bg-slate-100 text-slate-500 rounded-full shadow-sm"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0"/>
                <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7"/>
              </svg>
            </button>
          </div>
        </div>
      </div>

      <div class="pt-2 pb-4">
        <button
            v-if="colecao.produtos && colecao.produtos.length > 0"
            @click="addAllToCart"
            :class="isSuccess ? 'bg-emerald-500 shadow-emerald-200' : 'bg-slate-900 shadow-slate-900/20'"
            class="w-full text-white py-4 rounded-2xl font-black text-base shadow-xl active:scale-95 transition-all flex items-center justify-center gap-3"
        >
          <template v-if="!isSuccess">
            <i class="ph-bold ph-shopping-bag-open text-xl"></i>
            Adicionar Kit ao Carrinho
          </template>
          <template v-else>
            <i class="ph-bold ph-check-circle text-xl"></i>
            Kit Adicionado!
          </template>
        </button>
      </div>
    </div>
  </div>

  <div v-else class="p-20 text-center space-y-4">
    <i class="ph-bold ph-spinner animate-spin text-4xl text-slate-300"></i>
    <p class="text-sm font-bold text-slate-400 uppercase">Carregando detalhes do kit...</p>
  </div>
</template>

<style scoped>
.custom-scrollbar::-webkit-scrollbar { width: 4px; }
.custom-scrollbar::-webkit-scrollbar-thumb { background: #e2e8f0; border-radius: 10px; }
</style>