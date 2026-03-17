<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";
import ActionText from "../Components/Base/ActionText.vue";
import LoginButton from "../Components/Base/LoginButton.vue";
import { authService } from "../Services/AuthService.ts";

const router = useRouter();

const email = ref('');
const password = ref('');
const isLoading = ref(false);
const showPassword = ref(false);

const modalConfig = ref({
  show: false,
  title: '',
  message: '',
  type: 'success' as 'success' | 'error'
});

const notificar = (title: string, message: string, type: 'success' | 'error' = 'error') => {
  modalConfig.value = { show: true, title, message, type };
};

const handleLogin = async () => {
  if (!email.value || !password.value) {
    notificar("Campos Vazios", "Por favor, preencha seu e-mail e senha para entrar.", "error");
    return;
  }

  isLoading.value = true;

  try {
    const response = await authService.login({
      email: email.value,
      senha: password.value
    });

    if (response.success) {
      router.push('/');
    } else {
      notificar("Falha no Acesso", response.message || "Credenciais incorretas.", "error");
    }
  } catch (error) {
    notificar("Erro de Conexão", "Não foi possível contatar o servidor.", "error");
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <MobileLayoutBase :modal="modalConfig" @close-modal="modalConfig.show = false">
    <section class="flex flex-col items-center justify-center p-6 pt-12 h-full">
      <div class="w-full max-w-sm">

        <div class="text-center mb-10">
          <div class="w-20 h-20 bg-slate-900 rounded-[2rem] mx-auto mb-6 flex items-center justify-center shadow-2xl shadow-slate-200 rotate-3">
            <i class="ph-fill ph-lightning text-4xl text-white -rotate-3"></i>
          </div>
          <h1 class="text-3xl font-black text-slate-900 tracking-tight">Bem-vindo(a)</h1>
          <p class="text-slate-500 font-medium mt-2">Sua vitrine inteligente.</p>
        </div>

        <form @submit.prevent="handleLogin" class="space-y-5">
          <div class="space-y-2">
            <label class="block text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1">
              E-mail de Acesso
            </label>
            <div class="relative group">
              <i class="ph ph-envelope-simple absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 text-lg group-focus-within:text-slate-900 transition-colors"></i>
              <input
                  v-model="email"
                  type="email"
                  placeholder="Seu e-mail"
                  class="w-full pl-12 pr-4 py-4 rounded-2xl bg-slate-50 border border-slate-200 text-sm font-bold outline-none transition-all focus:bg-white focus:border-slate-900"
              />
            </div>
          </div>

          <div class="space-y-2">
            <label class="block text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1">
              Senha
            </label>
            <div class="relative group">
              <i class="ph ph-lock-key absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 text-lg group-focus-within:text-slate-900 transition-colors"></i>
              <input
                  v-model="password"
                  :type="showPassword ? 'text' : 'password'"
                  placeholder="Sua senha"
                  class="w-full pl-12 pr-12 py-4 rounded-2xl bg-slate-50 border border-slate-200 text-sm font-bold outline-none transition-all focus:bg-white focus:border-slate-900"
              />
              <button type="button" @click="showPassword = !showPassword" class="absolute right-4 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-900 transition-colors">
                <i class="ph-bold" :class="showPassword ? 'ph-eye-closed' : 'ph-eye'"></i>
              </button>
            </div>

            <div class="flex justify-end">
              <a href="#" class="text-xs font-bold text-slate-400 hover:text-slate-900">Esqueceu a senha?</a>
            </div>
          </div>

          <div class="pt-2">
            <LoginButton
                :is-loading="isLoading"
                text="Entrar no App"
                class="w-full bg-slate-900"
            />
          </div>
        </form>

        <div class="mt-8">
          <ActionText
              text="Ainda não tem conta?"
              action-label="Criar conta grátis"
              @action="router.push('/register')"
          />
        </div>
      </div>
    </section>
  </MobileLayoutBase>
</template>