<script setup lang="ts">
import { computed } from 'vue';

const name = defineModel<string>('name')
const username = defineModel<string>('username')

interface ProfileCardProps {
  userPhotoUrl?: string
  label?: string
}

const DEFAULT_AVATAR = 'https://cdn-icons-png.flaticon.com/512/149/149071.png';

const props = withDefaults(defineProps<ProfileCardProps>(), {
  userPhotoUrl: '',
  label: 'Conta Lumen'
})

const emit = defineEmits<{
  (e: 'edit-photo'): void
  (e: 'notify', title: string, message: string, type: 'success' | 'error'): void
}>()

const displayPhoto = computed(() => props.userPhotoUrl || DEFAULT_AVATAR);

const handleCopy = async () => {
  if (!username.value) return
  try {
    await navigator.clipboard.writeText(username.value)
    emit('notify', 'Copiado!', 'O nome de usuário foi copiado para a área de transferência.', 'success')
  } catch (error) {
    emit('notify', 'Erro', 'Não foi possível copiar o nome de usuário.', 'error')
  }
}
</script>

<template>
  <div class="bg-white p-5 rounded-3xl border border-slate-100 flex items-center gap-5 relative overflow-hidden shadow-sm">

    <div class="relative z-10 group cursor-pointer" @click="emit('edit-photo')">
      <div class="w-20 h-20 rounded-full p-1 bg-slate-900 shadow-md transition-transform active:scale-95">
        <img
            :src="displayPhoto"
            :alt="name"
            class="w-full h-full object-cover rounded-full border-2 border-white bg-slate-100"
        />
      </div>
      <div class="absolute bottom-0 right-0 bg-slate-900 text-white w-7 h-7 rounded-full flex items-center justify-center border-2 border-white shadow-sm transition-transform group-hover:scale-110">
        <i class="ph-bold ph-camera text-[10px]"></i>
      </div>
    </div>

    <div class="flex-1 z-10">
      <input
          v-model="name"
          type="text"
          placeholder="Seu Nome"
          class="w-full text-lg font-black text-slate-900 leading-tight bg-transparent border-none outline-none p-0 focus:ring-0"
      />

      <div class="flex items-center gap-2 mt-1">
        <button
            type="button"
            @click="handleCopy"
            class="flex items-center gap-2 text-slate-400 hover:text-slate-900 transition active:scale-95"
        >
          <i class="ph-bold ph-copy text-sm"></i>

          <div class="flex items-center text-xs font-bold text-slate-500 bg-slate-50 px-2 py-1 rounded-lg">
            <span class="mr-0.5 opacity-50">@</span>
            <input
                v-model="username"
                type="text"
                placeholder="usuario"
                class="bg-transparent border-none outline-none p-0 w-24 focus:ring-0 text-xs font-black text-slate-600"
            />
          </div>
        </button>
      </div>

      <p class="text-[9px] font-black uppercase tracking-[0.15em] text-slate-300 mt-2.5">
        {{ props.label }}
      </p>
    </div>

    <div class="absolute top-0 right-0 w-32 h-32 bg-slate-50 rounded-full blur-3xl -mr-10 -mt-10 opacity-40 pointer-events-none"></div>
  </div>
</template>