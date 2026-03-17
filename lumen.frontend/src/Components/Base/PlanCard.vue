<script setup lang="ts">
// No futuro você pode receber 'nomePlano' e 'dataRenovacao' via props
</script>

<template>
  <div class="relative overflow-hidden rounded-3xl bg-slate-900 p-6 text-white shadow-xl">
    <div class="relative z-10 flex justify-between items-start">
      <div>
        <span class="inline-block rounded-lg bg-white/20 px-2 py-1 text-[10px] font-bold uppercase backdrop-blur-md">
          Pro Plan
        </span>
        <h3 class="mt-2 text-xl font-bold">Lumen Pro</h3>
        <p class="mt-1 text-xs text-slate-400">Próxima renovação em 12/12</p>
      </div>
      <div class="h-10 w-10 rounded-full bg-white/10 flex items-center justify-center backdrop-blur-sm">
        <i class="ph-fill ph-crown text-yellow-400 text-xl"></i>
      </div>
    </div>

    <div class="absolute -right-6 -top-6 h-24 w-24 rounded-full bg-indigo-500/20 blur-2xl"></div>
    <div class="absolute -bottom-6 -left-6 h-32 w-32 rounded-full bg-indigo-500/10 blur-3xl"></div>
  </div>
</template>