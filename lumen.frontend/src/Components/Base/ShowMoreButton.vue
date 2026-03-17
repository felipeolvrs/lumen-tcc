<script setup lang="ts">
interface  ShowMoreButtonProps {
  text: string
}

withDefaults(defineProps<ShowMoreButtonProps>(), {
  text: ""
})
</script>

<template>
  <button
      class="w-full py-2 text-xs font-bold text-indigo-600 bg-indigo-50 rounded-lg hover:bg-indigo-100 transition mt-2"
  >
    {{text}}
  </button>
</template>

<style scoped>

</style>