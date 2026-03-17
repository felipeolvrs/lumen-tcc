<script setup lang="ts">
interface NavItemProps {
  text: string
  isActive?: boolean
}

withDefaults(defineProps<NavItemProps>(), {
  isActive: false
})

defineEmits(['click'])
</script>

<template>
  <a
      href="#"
      @click.prevent="$emit('click')"
      class="flex flex-col items-center gap-1 p-2 transition-colors duration-200"
      :class="isActive ? 'text-slate-900' : 'text-slate-400 hover:text-slate-900'"
  >
    <slot />

    <span
        class="text-[10px]"
        :class="isActive ? 'font-bold' : 'font-medium'"
    >
      {{ text }}
    </span>
  </a>
</template>