<script setup lang="ts">
interface ProductForm {
  name: string
  category: string
  price: number
  oldprice: number
  description: string
  review: string
}

interface Props {
  modelValue: ProductForm
  isEditMode?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  modelValue: () => ({
    name: '',
    category: 'own',
    link: "",
    price: 0,
    oldprice: 0,
    description: '',
    review: ''
  }),
  isEditMode: false
})

const emit = defineEmits(['update:modelValue', 'delete'])

const updateField = (field: keyof ProductForm, value: any) => {
  emit('update:modelValue', {
    ...props.modelValue,
    [field]: value
  })
}
</script>

<template>
  <div class="space-y-6 animate-in fade-in duration-500">

    <div class="group">
      <label class="block text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1 mb-2 group-focus-within:text-indigo-600 transition-colors">
        Nome do Produto
      </label>
      <input
          type="text"
          :value="props.modelValue.name"
          @input="updateField('name', ($event.target as HTMLInputElement).value)"
          placeholder="Ex: Headphone Bluetooth"
          class="w-full bg-slate-50 border border-slate-200 rounded-2xl py-4 px-5 text-sm font-bold text-slate-900 outline-none focus:border-indigo-600 focus:bg-white focus:ring-4 focus:ring-indigo-600/5 transition-all"
      />
    </div>

    <!--
  TODO: Implementar categorias
    <div>
      <label class="block text-xs font-bold text-slate-500 uppercase mb-2 ml-1">
        Categoria
      </label>
      <div class="relative">
        <select
            :value="props.modelValue.category"
            @change="updateField('category', ($event.target as HTMLSelectElement).value)"
            class="custom-input w-full bg-slate-50 border border-slate-200 rounded-2xl py-3.5 px-4 text-sm font-medium text-slate-900 outline-none appearance-none"
        >
          <option value="" disabled>Selecione...</option>
          <option>Eletrônicos</option>
          <option>Moda</option>
          <option>Casa & Decoração</option>
          <option>Tecnologia</option>
          <option>Beleza</option>
        </select>
        <div class="absolute right-4 top-1/2 -translate-y-1/2 pointer-events-none text-slate-400">
          <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M19 9l-7 7-7-7" />
          </svg>
        </div>
      </div>
    </div>
-->

    <div class="group">
      <label class="block text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1 mb-2 group-focus-within:text-indigo-600 transition-colors">
        Preço de Venda
      </label>
      <div class="relative">
        <div class="absolute left-5 top-1/2 -translate-y-1/2 flex items-center pointer-events-none">
          <span class="text-sm font-black text-slate-400 group-focus-within:text-indigo-600 transition-colors">R$</span>
        </div>
        <input
            type="number"
            step="0.01"
            min="0"
            :value="props.modelValue.price"
            @input="updateField('price', Number(($event.target as HTMLInputElement).value))"
            placeholder="0,00"
            class="w-full bg-slate-50 border border-slate-200 rounded-2xl py-4 pl-12 pr-5 text-sm font-black text-slate-900 outline-none focus:border-indigo-600 focus:bg-white focus:ring-4 focus:ring-indigo-600/5 transition-all"
        />
      </div>
    </div>

    <div class="group">
      <label class="block text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1 mb-2 group-focus-within:text-indigo-600 transition-colors">
        Descrição Detalhada
      </label>
      <textarea
          rows="4"
          :value="props.modelValue.description"
          @input="updateField('description', ($event.target as HTMLTextAreaElement).value)"
          placeholder="Descreva as principais qualidades..."
          class="w-full bg-slate-50 border border-slate-200 rounded-2xl py-4 px-5 text-sm font-bold text-slate-900 outline-none resize-none focus:border-indigo-600 focus:bg-white focus:ring-4 focus:ring-indigo-600/5 transition-all"
      ></textarea>
    </div>

    <div class="group bg-indigo-50/30 p-1 rounded-[22px] border border-transparent focus-within:border-indigo-100 transition-all">
      <label class="block text-[10px] font-black text-indigo-400 uppercase tracking-[0.2em] ml-4 mt-3 mb-2 flex justify-between pr-4">
        Review Pessoal
        <span class="text-[9px] font-bold bg-white text-indigo-400 px-2 py-0.5 rounded-full border border-indigo-50 shadow-sm">OPCIONAL</span>
      </label>
      <textarea
          rows="3"
          :value="props.modelValue.review"
          @input="updateField('review', ($event.target as HTMLTextAreaElement).value)"
          placeholder="O que você achou pessoalmente deste produto?"
          class="w-full bg-white/50 backdrop-blur-sm border border-indigo-50/50 rounded-[18px] py-4 px-5 text-sm font-medium text-slate-900 outline-none resize-none focus:bg-white focus:border-indigo-400 focus:ring-4 focus:ring-indigo-400/5 transition-all"
      ></textarea>
    </div>

    <div v-if="props.isEditMode" class="pt-4">
      <div class="h-px bg-slate-100 w-full mb-8"></div>
      <button
          type="button"
          @click="emit('delete')"
          class="w-full py-4 bg-white border-2 border-red-50 text-red-500 font-black text-[11px] uppercase tracking-widest flex items-center justify-center gap-3 hover:bg-red-50 hover:border-red-100 active:scale-95 transition-all rounded-2xl"
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="ph-bold ph-trash" viewBox="0 0 16 16">
          <path d="M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5m-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5M4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06m6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528M8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5"/>
        </svg>
        Excluir Produto Permanentemente
      </button>
    </div>
  </div>
</template>