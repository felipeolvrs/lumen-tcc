<script setup lang="ts">
interface ActionTextProps {
  text: string
  actionLabel: string
  to?: string | object
}

const props = withDefaults(defineProps<ActionTextProps>(), {
  text: '',
  actionLabel: '',
  to: ''
})

const emit = defineEmits(['action'])
</script>

<template>
  <p class="text-center text-xs text-slate-400 font-medium">
    <slot>
      {{ props.text }}

      <RouterLink
          v-if="props.to"
          :to="props.to"
          class="text-slate-900 font-bold hover:underline ml-1 transition-colors cursor-pointer"
      >
        {{ props.actionLabel }}
      </RouterLink>

      <button
          v-else-if="props.actionLabel"
          type="button"
          @click="emit('action')"
          class="text-slate-900 hover:underline ml-1 transition-colors bg-transparent border-none p-0 cursor-pointer font-bold"
      >
        {{ props.actionLabel }}
      </button>
    </slot>
  </p>
</template>