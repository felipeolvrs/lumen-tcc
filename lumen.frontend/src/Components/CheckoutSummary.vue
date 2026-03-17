<script setup lang="ts">
interface SummaryItem {
  id: string | number;
  nome: string;
  preco: number;
  quantidade: number;
}

const props = defineProps<{
  itens: SummaryItem[];
  total: number;
}>();

const formatCurrency = (value: number) => {
  return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
};
</script>

<template>
  <section class="bg-slate-50 rounded-3xl p-6 border border-slate-100 shadow-sm">
    <h3 class="text-[10px] font-black text-slate-400 uppercase tracking-[0.1em] mb-5">
      Resumo do Pedido
    </h3>

    <div class="space-y-4 mb-6 border-b border-slate-200/60 pb-6">
      <div
          v-for="item in props.itens"
          :key="item.id"
          class="flex justify-between items-start"
      >
        <div class="flex flex-col">
          <span class="text-sm font-bold text-slate-800">{{ item.nome }}</span>
          <span class="text-xs text-slate-500">{{ item.quantidade }}x {{ formatCurrency(item.preco) }}</span>
        </div>
        <span class="text-sm font-bold text-slate-900">
          {{ formatCurrency(item.preco * item.quantidade) }}
        </span>
      </div>

      <div v-if="props.itens.length === 0" class="text-center py-2 text-slate-400 text-xs italic">
        Nenhum item selecionado
      </div>
    </div>

    <div class="flex justify-between items-center">
      <span class="text-xs font-bold text-slate-500 uppercase">Total a Pagar</span>
      <span class="text-2xl font-black text-blue-900
      -600 tracking-tight">
        {{ formatCurrency(props.total) }}
      </span>
    </div>
  </section>
</template>