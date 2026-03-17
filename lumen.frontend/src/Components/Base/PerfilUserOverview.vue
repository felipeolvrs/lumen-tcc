<script setup lang="ts">
// Recebe se o usuário já é lojista para decidir se mostra o card ou não
defineProps<{
  isShopkeeper: boolean
}>()

const emit = defineEmits(['become-shopkeeper'])
</script>

<template>
  <div v-if="!isShopkeeper" class="animate-in fade-in slide-in-from-bottom-4 duration-500">
    <div
        class="bg-gradient-to-br from-indigo-600 to-indigo-900 rounded-3xl p-6 text-white shadow-xl relative overflow-hidden"
    >
      <div class="relative z-10">
        <h3 class="font-bold text-xl mb-1 text-white">Abra sua vitrine</h3>
        <p class="text-indigo-100 text-sm mb-5 leading-relaxed">
          Transforme suas indicações em renda extra com uma vitrine
          profissional e personalizada.
        </p>

        <button
            @click="$emit('become-shopkeeper')"
            class="bg-white text-indigo-700 text-sm font-bold px-5 py-3 rounded-xl shadow-lg active:scale-95 transition-all flex items-center gap-2"
        >
          <span>Quero ser Lojista</span>
          <i class="ph-bold ph-storefront"></i>
        </button>
      </div>

      <i
          class="ph-duotone ph-shopping-bag text-9xl absolute -right-6 -bottom-8 text-white opacity-10 rotate-12 pointer-events-none"
      ></i>
    </div>
  </div>
</template>