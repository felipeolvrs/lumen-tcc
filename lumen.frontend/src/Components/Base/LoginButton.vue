<script setup lang="ts">
interface LoginButtonProps {
  text: string
  isLoading: boolean // Corrigido nome e tipo
}

const props = withDefaults(defineProps<LoginButtonProps>(), {
  isLoading: false,
  text: 'Enviar'
})
</script>

<template>
  <button
      type="submit"
      :disabled="props.isLoading"
      class="w-full bg-slate-900 text-white font-bold py-4 rounded-2xl shadow-xl shadow-slate-900/20 transition-all hover:bg-slate-800 active:scale-95 flex items-center justify-center gap-2 mt-4 disabled:opacity-70 disabled:cursor-not-allowed"
  >
    <i v-if="props.isLoading" class="ph-bold ph-spinner animate-spin text-xl"></i>

    <span v-if="!props.isLoading">{{ props.text }}</span>

    <i v-if="!props.isLoading" class="ph-bold ph-arrow-right"></i>
  </button>
</template>