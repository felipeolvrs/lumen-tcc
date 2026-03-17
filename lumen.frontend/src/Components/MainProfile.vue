<script setup lang="ts">
import { type Produto } from '../Services/ProdutoService';
import { type ColecaoResponse } from '../Services/ColecaoService';

import SimpleButton from "./Base/SimpleButton.vue";
import SectionHeader from "./Base/SectionHeader.vue";
import VerticalCard from "./Base/VerticalCard.vue";
import SectionDivider from "./Base/SectionDivider.vue";
import ProductCard from "./Base/ProductCard.vue";

const props = defineProps<{
  produtos: Produto[];
  colecoes: ColecaoResponse[];
  isLoading: boolean;
}>();

const emit = defineEmits<{
  'open-product': [produto: any];
  'open-items-colecao': [colecao: any];
  'open-colecao': [];
}>();

const DEFAULT_IMAGE = "https://placehold.co/400x400/f1f5f9/94a3b8?text=Sem+Imagem";

const formatPrice = (value: number | string) => {
  if (typeof value === 'string') return value;
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(value);
};
</script>

<template>
  <div class="bg-white min-h-screen pb-20">

    <div v-if="isLoading" class="p-6 space-y-10 animate-pulse">
      <div class="space-y-4">
        <div class="h-8 w-1/2 bg-slate-100 rounded-full"></div>
        <div class="flex gap-4 overflow-hidden">
          <div v-for="i in 3" :key="i" class="w-40 h-56 bg-slate-50 rounded-3xl flex-shrink-0"></div>
        </div>
      </div>
      <div class="space-y-6">
        <div class="h-6 w-full bg-slate-50 rounded-full"></div>
        <div class="grid grid-cols-2 gap-4">
          <div v-for="i in 4" :key="i" class="aspect-[4/5] bg-slate-50 rounded-3xl"></div>
        </div>
      </div>
    </div>

    <template v-else>
      <section
          v-if="colecoes.length > 0"
          class="py-6 bg-white animate-in fade-in slide-in-from-bottom-4 duration-700"
      >
        <div class="px-6 mb-6 flex items-end justify-between">
          <SectionHeader
              title="Kits em Destaque"
              subtitle="Coleções selecionadas para você"
          />
          <SimpleButton text="Ver Todos" @click="emit('open-colecao')" />
        </div>

        <div class="flex gap-4 overflow-x-auto px-6 pb-4 scrollbar-hide snap-x scroll-pl-6">
          <VerticalCard
              v-for="item in colecoes"
              :key="item.id"
              :image="item.urlCapa || DEFAULT_IMAGE"
              :title="item.nome"
              :category="item.plataforma || 'Kit Especial'"
              class="snap-start"
              @click="emit('open-items-colecao', item)"
          />
        </div>
      </section>

      <section class="px-5 pb-12 animate-in fade-in slide-in-from-bottom-6 duration-1000">
        <div class="py-6 bg-white">
          <SectionDivider text="Vitrine de Produtos" />
        </div>

        <div v-if="produtos.length > 0" class="px-1 grid grid-cols-2 gap-x-4 gap-y-10">
          <ProductCard
              v-for="produto in produtos"
              :key="produto.id"
              :image="produto.urlImagem || DEFAULT_IMAGE"
              :title="produto.nome"
              :price="formatPrice(produto.preco)"
              @click="emit('open-product', produto)"
          />
        </div>

        <div v-else class="text-center py-24 bg-slate-50 rounded-[40px] border-2 border-dashed border-slate-100 mx-1">
          <div class="w-16 h-16 bg-white rounded-full flex items-center justify-center mx-auto shadow-sm mb-4">
            <i class="ph-fill ph-package text-3xl text-slate-200"></i>
          </div>
          <h3 class="text-slate-900 font-black text-sm uppercase tracking-widest">Vitrine Vazia</h3>
          <p class="text-slate-400 text-[10px] font-bold mt-1 uppercase">Aguardando novos produtos</p>
        </div>
      </section>
    </template>
  </div>
</template>

<style scoped>
.scrollbar-hide::-webkit-scrollbar {
  display: none;
}
.scrollbar-hide {
  -ms-overflow-style: none;
  scrollbar-width: none;
}
</style>