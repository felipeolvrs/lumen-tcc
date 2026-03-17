<script setup lang="ts">
interface SimpleButtonProps {
  text: string
}

withDefaults(defineProps<SimpleButtonProps>(), {
  text: ""
})

const emit = defineEmits(['to'])
</script>

<template>
  <button @click="$emit('to')" class="text-xs font-semibold text-slate-400 hover:text-indigo-600 transition flex items-center hover:underline">
    <slot>{{text}}</slot>
  </button>
</template>

<style scoped>

</style>