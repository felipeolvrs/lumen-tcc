<script setup lang="ts">
interface Props {
  modelValue: {
    tipoEntrega: string;
    endereco: string;
  };
}

const props = defineProps<Props>();
</script>

<template>
  <fieldset>
    <legend class="text-sm font-bold text-slate-900 uppercase tracking-wider mb-4 flex items-center gap-2">
      <span class="w-6 h-6 rounded-full bg-slate-900 text-white flex items-center justify-center text-xs">2</span>
      Entrega
    </legend>

    <div class="grid grid-cols-2 gap-3 mb-4">
      <label class="cursor-pointer">
        <input type="radio" value="pickup" v-model="props.modelValue.tipoEntrega" class="peer sr-only">
        <div class="p-3 rounded-xl border-2 border-slate-200 bg-white peer-checked:border-emerald-600 peer-checked:bg-emerald-50 text-center transition h-full">
          <span class="block text-sm font-bold text-slate-700">Retirar</span>
          <span class="text-[10px] text-emerald-600 font-bold uppercase">Grátis</span>
        </div>
      </label>

      <label class="cursor-pointer">
        <input type="radio" value="delivery" v-model="props.modelValue.tipoEntrega" class="peer sr-only">
        <div class="p-3 rounded-xl border-2 border-slate-200 bg-white peer-checked:border-b-blue-900 peer-checked:bg-indigo-50 text-center transition h-full">
          <span class="block text-sm font-bold text-slate-700">Entrega</span>
          <span class="text-[10px] text-blue-900 font-bold uppercase">A combinar</span>
        </div>
      </label>
    </div>

    <div v-if="props.modelValue.tipoEntrega === 'delivery'" class="animate-in fade-in slide-in-from-top-2">
      <label class="block text-xs font-bold text-slate-500 mb-1">Endereço de Entrega</label>
      <textarea
          v-model="props.modelValue.endereco"
          rows="2"
          placeholder="Rua, Número, Bairro e Complemento..."
          class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:border-blue-900 outline-none transition resize-none"
      ></textarea>
    </div>
  </fieldset>
</template>