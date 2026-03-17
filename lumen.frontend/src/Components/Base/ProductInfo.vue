<script setup lang="ts">
interface ProductInfoProps {
  tituloProduto: string
  descricaoProduto: string
  preco: string
}

defineProps<ProductInfoProps>()
</script>

<template>
  <div class="flex flex-col space-y-6">

    <header class="space-y-4">
      <h1 class="text-3xl font-extrabold text-blue-950 leading-tight tracking-tight">
        {{ tituloProduto }}
      </h1>

      <p class="text-sm font-medium text-slate-500 leading-relaxed">
        {{ descricaoProduto }}
      </p>
    </header>

    <section class="pb-6 border-b border-slate-100 flex flex-col items-start mt-2">
      <span class="text-[10px] font-black text-slate-400 uppercase tracking-widest mb-1 ml-1">
        Preço atual
      </span>
      <p class="text-5xl font-black text-blue-950 tracking-tighter">
        {{ preco }}
      </p>
    </section>

  </div>
</template>