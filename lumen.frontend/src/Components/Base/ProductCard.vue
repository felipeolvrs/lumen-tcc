<script setup lang="ts">
import { computed } from 'vue'

interface ProductCardProps {
  image: string
  title: string
  price: string | number
}

const props = withDefaults(defineProps<ProductCardProps>(), {})

const DEFAULT_IMAGE = "https://placehold.co/400x400/f1f5f9/94a3b8?text=Sem+Foto";

const formattedPrice = computed(() => {
  if (typeof props.price === 'number') {
    return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(props.price)
  }
  return props.price
})
</script>

<template>
  <div class="group cursor-pointer bg-white border border-slate-100 rounded-2xl p-2.5 hover:shadow-lg hover:shadow-slate-200/40 hover:border-slate-200 transition-all duration-300">

    <div class="aspect-square bg-slate-50 relative overflow-hidden rounded-xl group-active:scale-[0.97] transition-transform">
      <img
          :src="image || DEFAULT_IMAGE"
          :alt="title"
          loading="lazy"
          class="w-full h-full object-cover group-hover:scale-105 transition-transform duration-500"
      />
    </div>

    <div class="mt-3.5 px-1 pb-1">
      <h3 class="text-[12px] font-bold text-slate-600 leading-tight line-clamp-2 mb-2 min-h-[2.4em] group-hover:text-slate-900 transition-colors">
        {{ title }}
      </h3>

      <div class="flex items-center justify-between mt-auto">
        <p class="text-[15px] font-black text-slate-900 tracking-tight">
          {{ formattedPrice }}
        </p>

        <div class="text-slate-300 group-hover:text-slate-900 transition-colors">
          <i class="ph-bold ph-plus-circle text-lg"></i>
        </div>
      </div>
    </div>
  </div>
</template>