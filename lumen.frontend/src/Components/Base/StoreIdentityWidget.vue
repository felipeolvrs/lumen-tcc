<script setup lang="ts">
// Definimos os modelos que o pai está enviando
const nomeLoja = defineModel<string>('nomeLoja')
const bio = defineModel<string>('bio')
const whatsapp = defineModel<string>('whatsapp')
const instagram = defineModel<string>('instagram')
const chavepix = defineModel<string>('chavepix')
const slug = defineModel<string>('slug')
</script>

<template>
  <div class="bg-white rounded-3xl shadow-soft border border-slate-100 overflow-hidden">

    <div class="p-5 border-b border-slate-50">
      <label class="block text-xs font-bold text-slate-400 uppercase mb-2">
        Nome da Loja
      </label>
      <input
          v-model="nomeLoja"
          type="text"
          placeholder="Ex: Minha Vitrine"
          class="w-full font-bold text-slate-900 outline-none bg-transparent placeholder-slate-300 text-base"
      />
    </div>

    <div class="p-5 border-b border-slate-50">
      <label class="block text-xs font-bold text-slate-400 uppercase mb-2">
        Bio do Perfil
      </label>
      <textarea
          v-model="bio"
          rows="2"
          placeholder="Conte um pouco sobre sua loja..."
          class="w-full text-sm font-medium text-slate-600 outline-none bg-transparent resize-none placeholder-slate-300 leading-relaxed"
      ></textarea>
    </div>

    <div class="p-5 border-b border-slate-50">
      <label class="block text-xs font-bold text-slate-400 uppercase mb-2">
        Whatsapp
      </label>
      <input
          v-model="whatsapp"
          type="text"
          placeholder="Ex: 91234-5678"
          class="w-full font-bold text-base text-slate-900 outline-none bg-transparent placeholder-slate-300"
      />
    </div>

    <div class="p-5 border-b border-slate-50">
      <label class="block text-xs font-bold text-slate-400 uppercase mb-2">
        Instagram
      </label>
      <input
          v-model="instagram"
          type="text"
          placeholder="Ex: @minhaloja"
          class="w-full font-bold text-base text-slate-900 outline-none bg-transparent placeholder-slate-300"
      />
    </div>

    <div class="p-5 border-b border-slate-50">
      <label class="block text-xs font-bold text-slate-400 uppercase mb-2">
        Chave Pix
      </label>
      <input
          v-model="chavepix"
          type="text"
          placeholder=""
          class="w-full font-bold text-base text-slate-900 outline-none bg-transparent placeholder-slate-300"
      />
    </div>

    <div class="p-5 bg-slate-50/50 flex items-center gap-2">
      <i class="ph-bold ph-link text-slate-400 text-lg"></i>
      <span class="text-sm text-slate-400 font-medium">lumen.app/</span>
      <input
          v-model="slug"
          type="text"
          placeholder="seu-usuario"
          class="w-full text-base font-bold text-slate-900 outline-none bg-transparent focus:text-indigo-600 transition-colors"
      />
    </div>

  </div>
</template>