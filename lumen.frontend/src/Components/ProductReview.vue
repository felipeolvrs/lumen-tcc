<script setup lang="ts">
import { computed } from 'vue';

interface ProductReviewProps {
  userName: string
  sectionTitle: string
  imageUser?: string
  createdAt: Date
  comment: string
}

const props = defineProps<ProductReviewProps>();

const initials = computed(() => {
  return props.userName
      .split(' ')
      .map(n => n[0])
      .join('')
      .toUpperCase()
      .slice(0, 2);
});
</script>

<template>
  <section class="space-y-4 pt-2">
    <h3 class="text-sm font-bold text-slate-900 uppercase tracking-wider flex items-center gap-2">
      <span class="w-1.5 h-1.5 bg-slate-300 rounded-full"></span>
      {{sectionTitle}}
    </h3>

    <div class="space-y-4">
      <div class="flex gap-3">
        <div class="w-8 h-8 rounded-full bg-slate-100 flex-shrink-0 flex items-center justify-center text-xs font-bold text-slate-500 overflow-hidden">
          <img v-if="imageUser" :src="imageUser" class="w-full h-full object-cover" />
          <span v-else>{{ initials }}</span>
        </div>

        <div class="flex-1">
          <div class="flex items-baseline justify-between w-full">
            <span class="text-xs font-bold text-slate-900">{{ userName }}</span>
            <span class="text-[10px] text-slate-400 ml-2">Há 2 dias</span>
          </div>

          <div class="flex text-amber-400 text-[10px] mb-1">★★★★★</div>

          <p class="text-xs text-slate-600 leading-snug">
            {{ comment }}
          </p>
        </div>
      </div>
    </div>
  </section>
</template>