<script setup lang="ts">
interface ConfigHeaderProps {
  title: string
}

const props = withDefaults(defineProps<ConfigHeaderProps>(), {
  title: ""
})
</script>

<template>
  <header
      class="top-0 left-0 right-0 z-30 bg-white/80 backdrop-blur-xl border-b border-slate-200/50 max-w-md mx-auto"
  >
    <div class="px-6 h-[70px] flex items-center justify-between pt-2">
      <h1 class="text-lg font-bold text-slate-900">{{props.title}}</h1>
    </div>
  </header>
</template>

<style scoped>

</style>