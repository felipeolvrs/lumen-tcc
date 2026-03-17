<script setup lang="ts">
// Definimos as props para receber o e-mail dinamicamente
interface SettingsAccountProps {
  email?: string
}

const props = defineProps<SettingsAccountProps>()

// Definimos eventos para caso você queira abrir modais de edição no futuro
const emit = defineEmits<{
  (e: 'change-password'): void
  (e: 'change-email'): void
}>()
</script>

<template>
  <div class="animate-in fade-in slide-in-from-bottom-3 duration-500">
    <h3
        class="text-xs font-bold text-slate-400 uppercase tracking-wider mb-3 px-1"
    >
      Segurança
    </h3>
    <div
        class="bg-white rounded-3xl shadow-soft border border-slate-100 overflow-hidden"
    >
      <button
          @click="emit('change-email')"
          class="w-full flex items-center justify-between p-5 border-b border-slate-50 hover:bg-slate-50 transition active:scale-[0.99]"
      >
        <div class="flex items-center gap-3">
          <div
              class="w-8 h-8 rounded-full bg-slate-100 flex items-center justify-center text-slate-500"
          >
            <i class="ph-bold ph-envelope text-lg"></i>
          </div>
          <div class="text-left">
            <span class="block text-[10px] font-bold text-slate-400 uppercase">E-mail</span>
            <span class="block text-sm font-bold text-slate-900 truncate max-w-[180px]">
              {{ props.email || 'Não informado' }}
            </span>
          </div>
        </div>
        <i class="ph-bold ph-caret-right text-slate-300"></i>
      </button>

      <button
          @click="emit('change-password')"
          class="w-full flex items-center justify-between p-5 hover:bg-slate-50 transition active:scale-[0.99]"
      >
        <div class="flex items-center gap-3">
          <div
              class="w-8 h-8 rounded-full bg-slate-100 flex items-center justify-center text-slate-500"
          >
            <i class="ph-bold ph-lock-key text-lg"></i>
          </div>
          <div class="text-left">
            <span class="block text-[10px] font-bold text-slate-400 uppercase">Senha</span>
            <span class="block text-sm font-bold text-slate-900">••••••••••</span>
          </div>
        </div>
        <i class="ph-bold ph-caret-right text-slate-300"></i>
      </button>

    </div>
  </div>
</template>