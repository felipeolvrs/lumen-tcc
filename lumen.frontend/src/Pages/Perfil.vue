<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { useRouter } from "vue-router";
import { usuarioService, type UsuarioPerfil } from "../Services/UsuarioService.ts";
import { authService } from "../Services/AuthService.ts";

import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";
import ConfigHeader from "../Components/Base/ConfigHeader.vue";
import PerfilCard from "../Components/Base/PerfilCard.vue";
import NavbarBar from "../Components/Base/NavbarBar.vue";
import PerfilAdminOverview from "../Components/Base/PerfilAdminOverview.vue";
import SettingsAccount from "../Components/Base/SettingsAccount.vue";
import ButtonSheetBase from "../Components/Base/ButtonSheetBase.vue";

const router = useRouter();

const isLoading = ref(false);
const isDataFetching = ref(true);
const isSavedSuccess = ref(false);
const isBusinessDrawerOpen = ref(false);
const fileInput = ref<HTMLInputElement | null>(null);

const DEFAULT_AVATAR = "https://cdn-icons-png.flaticon.com/512/149/149071.png";

const modalConfig = ref({
  show: false,
  title: '',
  message: '',
  type: 'success' as 'success' | 'error'
});

const notificar = (title: string, message: string, type: 'success' | 'error' = 'success') => {
  modalConfig.value = { show: true, title, message, type };
};

const profile = ref<UsuarioPerfil>({
  nome: '',
  username: '',
  email: '',
  fotoUrl: '',
  whatsapp: '',
  tipoLoja: 'hybrid',
  bio: '',
  nomeLoja: '',
  ativo: true
});

const displayPhoto = computed(() => profile.value.fotoUrl || DEFAULT_AVATAR);

const fetchProfile = async () => {
  isDataFetching.value = true;
  try {
    const { data } = await usuarioService.obterPerfil();
    if (data) {
      profile.value = {
        nome: data.nome || '',
        email: data.email || '',
        username: (data as any).slug || data.username || '',
        fotoUrl: (data as any).urlLogo || data.fotoUrl || '',
        whatsapp: (data as any).whatsApp || data.whatsapp || '',
        tipoLoja: data.tipoLoja || 'hybrid',
        bio: data.bio || '',
        nomeLoja: data.nomeLoja || '',
        ativo: data.ativo !== undefined ? data.ativo : true,
        instagram: data.instagram || '',
        urlBanner: data.urlBanner || ''
      };
    }
  } catch (error) {
    notificar("Erro", "Não conseguimos carregar seus dados de perfil.", "error");
  } finally {
    isDataFetching.value = false;
  }
};

const saveChanges = async () => {
  if (isLoading.value) return;

  isLoading.value = true;
  isSavedSuccess.value = false;
  try {
    await usuarioService.atualizarPerfil(profile.value);
    isSavedSuccess.value = true;
    isBusinessDrawerOpen.value = false;

    notificar("Sucesso!", "Suas alterações foram salvas com sucesso.", "success");

    setTimeout(() => isSavedSuccess.value = false, 3000);
  } catch (error) {
    notificar("Falha ao Salvar", "Verifique sua conexão ou se o nome de usuário já está em uso.", "error");
  } finally {
    isLoading.value = false;
  }
};

const abrirSeletorDeImagem = () => fileInput.value?.click();

const handleFileUpload = (event: Event) => {
  const target = event.target as HTMLInputElement;
  const file = target.files?.[0];

  if (file) {
    const reader = new FileReader();
    reader.onload = (e) => {
      profile.value.fotoUrl = e.target?.result as string;
    };
    reader.readAsDataURL(file);
  }
};

const handleLogout = () => {
  authService.logout();
};

onMounted(fetchProfile);
</script>

<template>
  <MobileLayoutBase :modal="modalConfig" @close-modal="modalConfig.show = false">
    <ConfigHeader title="Configurações" />

    <input type="file" ref="fileInput" class="hidden" accept="image/*" @change="handleFileUpload" />

    <div class="px-5 py-6 space-y-8 pb-32">
      <div v-if="isDataFetching" class="space-y-6 animate-pulse">
        <div class="h-32 bg-slate-100 rounded-3xl w-full"></div>
        <div class="h-64 bg-slate-100 rounded-3xl w-full"></div>
      </div>

      <template v-else>
        <PerfilCard
            v-model:name="profile.nome"
            v-model:username="profile.username"
            :user-photo-url="displayPhoto"
            label="Perfil Oficial"
            @edit-photo="abrirSeletorDeImagem"
        />

        <PerfilAdminOverview
            v-model:ativo="profile.ativo"
            v-model:tipoLoja="profile.tipoLoja"
            v-model:nomeLoja="profile.nomeLoja"
            v-model:bio="profile.bio"
            v-model:whatsapp="profile.whatsapp"
            v-model:slug="profile.username"
            v-model:chavepix="profile.chavepix"
            @open-business="isBusinessDrawerOpen = true"
        />

        <SettingsAccount :email="profile.email" />

        <div class="space-y-3 pt-4">
          <button
              @click="saveChanges"
              :disabled="isLoading"
              class="w-full h-16 rounded-2xl font-bold shadow-lg transition-all flex items-center justify-center gap-2 active:scale-95 disabled:opacity-50"
              :class="isSavedSuccess ? 'bg-emerald-500 text-white' : 'bg-slate-900 text-white'"
          >
            <i v-if="isLoading" class="ph ph-spinner animate-spin text-xl"></i>
            <i v-else-if="isSavedSuccess" class="ph-bold ph-check-circle text-xl"></i>
            <span>{{ isLoading ? 'Salvando...' : (isSavedSuccess ? 'Atualizado!' : 'Salvar Alterações') }}</span>
          </button>

          <button @click="handleLogout" class="w-full text-rose-500 font-bold py-4 rounded-2xl active:scale-95 transition hover:bg-rose-50">
            Sair da conta
          </button>
        </div>
      </template>
    </div>

    <template #footer>
      <NavbarBar
          :user-avatar="displayPhoto"
          @go-home="() => router.push('/')"
          @open-kits="() => router.push('/colecao')"
          @open-perfil="() => {}"
          @open-add="() => isBusinessDrawerOpen = false"
      />
    </template>

    <ButtonSheetBase :is-open="isBusinessDrawerOpen" @close="isBusinessDrawerOpen = false">
      <div class="p-6">
        <h3 class="text-xl font-bold text-slate-900">Configurar Vitrine</h3>
        <p class="text-sm text-slate-500 mb-6">Selecione seu modelo de negócio.</p>

        <div class="space-y-3">
          <label v-for="opt in [{id:'products', l:'Produtos', i:'ph-package'}, {id:'affiliate', l:'Afiliado', i:'ph-megaphone'}, {id:'hybrid', l:'Híbrido', i:'ph-intersect'}]"
                 :key="opt.id" class="flex items-center p-4 border rounded-2xl gap-4 cursor-pointer"
                 :class="profile.tipoLoja === opt.id ? 'border-slate-900 bg-slate-50 ring-1 ring-slate-900' : 'border-slate-100'">
            <i :class="['ph-bold', opt.i, 'text-xl text-slate-600']"></i>
            <span class="flex-1 font-bold text-slate-800 text-sm">{{ opt.l }}</span>
            <input type="radio" :value="opt.id" v-model="profile.tipoLoja" class="accent-slate-900 w-5 h-5">
          </label>
        </div>

        <div class="mt-8 p-5 bg-slate-50 rounded-3xl border border-slate-100">
          <label class="block text-[10px] font-black text-slate-400 uppercase mb-2 ml-1">WhatsApp de Contato</label>
          <input v-model="profile.whatsapp" type="tel" placeholder="(00) 00000-0000" class="w-full bg-white border border-slate-200 rounded-2xl px-4 py-4 text-sm font-bold outline-none focus:border-slate-900">
        </div>

        <button @click="saveChanges" class="w-full bg-slate-900 text-white py-4 rounded-2xl mt-8 font-bold shadow-xl active:scale-95">
          Confirmar e Salvar
        </button>
      </div>
    </ButtonSheetBase>
  </MobileLayoutBase>
</template>