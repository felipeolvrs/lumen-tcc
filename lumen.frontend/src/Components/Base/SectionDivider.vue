<script setup lang="ts">
interface SectionDividerProps {
  text: string
}

withDefaults(defineProps<SectionDividerProps>(), {
  text: "text"
})
</script>

<template>
  <div class="flex items-center justify-center mb-8">
    <div class="h-px bg-slate-100 w-full"></div>
    <span class="px-3 text-xs text-slate-400 font-medium whitespace-nowrap uppercase tracking-wider">
      {{ text }}
    </span>
    <div class="h-px bg-slate-100 w-full"></div>
  </div>
</template>

<style scoped>

</style>