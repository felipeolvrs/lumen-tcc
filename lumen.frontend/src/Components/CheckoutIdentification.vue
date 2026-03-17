<script setup lang="ts">
interface UserData {
  nome: string;
  telefone: string;
  email: string;
}

interface Props {
  modelValue: UserData;
  errors?: { nome?: string; telefone?: string };
}

const props = defineProps<Props>();
</script>

<template>
  <fieldset class="space-y-4">
    <legend class="text-sm font-bold text-slate-900 uppercase tracking-wider mb-4 flex items-center gap-2">
      <span class="w-6 h-6 rounded-full bg-slate-900 text-white flex items-center justify-center text-xs">1</span>
      Quem é você?
    </legend>

    <div>
      <label class="block text-xs font-bold text-slate-500 mb-1">Nome Completo</label>
      <input
          v-model="props.modelValue.nome"
          type="text"
          placeholder="Seu nome"
          class="w-full bg-slate-50 border rounded-xl px-4 py-3 text-sm focus:border-b-blue-900 outline-none transition"
          :class="errors?.nome ? 'border-red-500' : 'border-slate-200'"
      />
      <span v-if="errors?.nome" class="text-[10px] text-red-500 font-bold ml-1">{{ errors.nome }}</span>
    </div>

    <div class="grid grid-cols-2 gap-4">
      <div>
        <label class="block text-xs font-bold text-slate-500 mb-1">WhatsApp</label>
        <input
            v-model="props.modelValue.telefone"
            type="tel"
            placeholder="(11) 99999..."
            class="w-full bg-slate-50 border rounded-xl px-4 py-3 text-sm focus:border-blue-900 outline-none transition"
            :class="errors?.telefone ? 'border-red-500' : 'border-slate-200'"
        />
      </div>
      <div>
        <label class="block text-xs font-bold text-slate-500 mb-1">E-mail (opcional)</label>
        <input
            v-model="props.modelValue.email"
            type="email"
            placeholder="seu@email.com"
            class="w-full bg-slate-50 border border-slate-200 rounded-xl px-4 py-3 text-sm focus:border-blue-900 outline-none transition"
        />
      </div>
    </div>
  </fieldset>
</template>