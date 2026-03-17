<script setup lang="ts">
import { computed } from 'vue'

type ColorVariant = 'indigo' | 'amber' | 'blue' | 'green' | 'slate';

interface ActionCardProps {
  title: string
  subtitle: string
  color?: ColorVariant
  icon?: string
}

const props = withDefaults(defineProps<ActionCardProps>(), {
  color: 'indigo',
  icon: ''
})

const emit = defineEmits<{
  (e: 'click'): void
}>()

const colorStyles: Record<ColorVariant, string> = {
  indigo: 'bg-indigo-100 text-indigo-600 group-hover:bg-indigo-600',
  amber:  'bg-amber-100 text-amber-600 group-hover:bg-amber-600',
  blue:   'bg-blue-100 text-blue-600 group-hover:bg-blue-600',
  green:  'bg-emerald-100 text-emerald-600 group-hover:bg-emerald-600',
  slate:  'bg-slate-100 text-slate-600 group-hover:bg-slate-600',
}

const iconClasses = computed(() => {
  return colorStyles[props.color] || colorStyles.indigo
})
</script>

<template>
  <button
      @click="emit('click')"
      class="flex items-center gap-4 p-4 rounded-2xl border border-slate-200
           hover:border-slate-900 hover:bg-slate-50
           transition-all duration-200 active:scale-[0.98]
           group text-left w-full"
  >
    <div
        class="w-12 h-12 rounded-full flex items-center justify-center
             group-hover:text-white transition-colors duration-200 flex-shrink-0"
        :class="iconClasses"
    >
      <img
          v-if="props.icon"
          :src="props.icon"
          alt="ícone"
          class="w-6 h-6 object-contain"
      />

      <slot v-else name="icon">
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-6 h-6">
          <path stroke-linecap="round" stroke-linejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
        </svg>
      </slot>

    </div>

    <div>
      <span class="block font-bold text-slate-900">{{ title }}</span>
      <span class="text-xs text-slate-500">{{ subtitle }}</span>
    </div>
  </button>
</template>