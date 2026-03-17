<script setup lang="ts">
interface Props {
  modelValue: {
    pagamento: string;
    observacao: string;
  };
}

const props = defineProps<Props>();
</script>

<template>
  <fieldset>
    <legend class="text-sm font-bold text-slate-900 uppercase tracking-wider mb-4 flex items-center gap-2">
      <span class="w-6 h-6 rounded-full bg-slate-900 text-white flex items-center justify-center text-xs">3</span>
      Pagamento
    </legend>

    <label class="relative block mb-4 cursor-pointer">
      <input type="radio" value="pix" v-model="props.modelValue.pagamento" class="peer sr-only">
      <div class="p-4 rounded-xl border-2 border-slate-200 bg-white peer-checked:border-emerald-500 peer-checked:bg-emerald-50/30 flex items-center gap-4 transition">
        <div class="w-5 h-5 rounded-full border-2 border-slate-300 bg-white peer-checked:border-[6px] peer-checked:border-emerald-500"></div>
        <div class="flex-1">
          <span class="font-bold text-slate-900 block">Pix</span>
          <span class="text-xs text-slate-500">Chave enviada no WhatsApp</span>
        </div>
        <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-emerald-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M13 10V3L4 14h7v7l9-11h-7z" />
        </svg>
      </div>
    </label>

    <div>
      <label class="block text-xs font-bold text-slate-500 mb-1">Observações (Opcional)</label>
      <input
          v-model="props.modelValue.observacao"
          type="text"
          placeholder="Ex: Embrulhar para presente"
          class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:border-indigo-500 outline-none transition"
      />
    </div>
  </fieldset>
</template>