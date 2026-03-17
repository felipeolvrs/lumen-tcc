<script setup lang="ts">
interface SectionHeaderProps {
  title: string
  subtitle: string
}

withDefaults(defineProps<SectionHeaderProps>(), {
  title: "",
  subtitle: ""
})

</script>

<template>
  <div>
    <h2 class="text-base font-bold text-slate-900 tracking-tight">
      {{title}}
    </h2>
    <p class="text-xs text-slate-500 mt-0.5">
      {{subtitle}}
    </p>
  </div>
</template>

<style scoped>

</style>