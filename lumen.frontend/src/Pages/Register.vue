<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { authService } from "../Services/AuthService";

import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";
import ActionText from "../Components/Base/ActionText.vue";
import LoginButton from "../Components/Base/LoginButton.vue";

const router = useRouter();
const isLoading = ref(false);
const showPassword = ref(false);

const modalConfig = ref({
  show: false,
  title: '',
  message: '',
  type: 'success' as 'success' | 'error'
});

const notificar = (title: string, message: string, type: 'success' | 'error' = 'success') => {
  modalConfig.value = { show: true, title, message, type };
};

const form = ref({
  name: '',
  email: '',
  password: '',
  confirmPassword: ''
});

const errors = ref({
  name: '',
  email: '',
  password: '',
  confirmPassword: ''
});

const validateForm = () => {
  let valid = true;
  errors.value = { name: '', email: '', password: '', confirmPassword: '' };

  if (form.value.name.length < 3) {
    errors.value.name = "Nome muito curto";
    valid = false;
  }
  if (!form.value.email.includes('@')) {
    errors.value.email = "E-mail inválido";
    valid = false;
  }
  if (form.value.password.length < 6) {
    errors.value.password = "Mínimo 6 caracteres";
    valid = false;
  }
  if (form.value.password !== form.value.confirmPassword) {
    errors.value.confirmPassword = "As senhas não coincidem";
    valid = false;
  }
  return valid;
};

const handleRegister = async () => {
  if (!validateForm()) return;

  isLoading.value = true;

  try {
    const payload = {
      nome: form.value.name,
      email: form.value.email,
      senha: form.value.password
    };

    await authService.cadastrar(payload);

    notificar("Conta Criada!", "Sua jornada no Lumen começou. Agora, faça o seu login.", "success");

    setTimeout(() => {
      router.push('/login');
    }, 1000);

  } catch (error: any) {
    if (error.response?.status === 400 || error.response?.data?.message?.includes('E-mail')) {
      errors.value.email = "E-mail já cadastrado no sistema";
      notificar("Erro no Cadastro", "Este e-mail já possui uma conta vinculada.", "error");
    } else {
      notificar("Falha no Registro", "Não conseguimos criar sua conta. Tente novamente em instantes.", "error");
    }
  } finally {
    isLoading.value = false;
  }
};
</script>

<template>
  <MobileLayoutBase :modal="modalConfig" @close-modal="modalConfig.show = false">
    <section class="p-6 pt-12 flex flex-col items-center justify-center">
      <div class="w-full max-w-sm">

        <div class="text-center mb-8">
          <div class="w-20 h-20 bg-slate-900 rounded-[2rem] mx-auto mb-6 flex items-center justify-center shadow-2xl shadow-slate-200 rotate-6">
            <i class="ph-fill ph-rocket-launch text-4xl text-white"></i>
          </div>
          <h1 class="text-3xl font-black text-slate-900 tracking-tight">Criar Conta</h1>
          <p class="text-slate-500 font-medium mt-2">Sua vitrine profissional começa aqui.</p>
        </div>

        <form @submit.prevent="handleRegister" class="space-y-4">
          <div class="space-y-1">
            <div class="relative group">
              <i class="ph ph-user absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 text-lg group-focus-within:text-slate-900 transition-colors"></i>
              <input v-model="form.name" type="text" placeholder="Nome Completo"
                     class="w-full pl-12 pr-4 py-4 rounded-2xl bg-slate-50 border border-slate-200 text-sm font-bold outline-none focus:bg-white focus:border-slate-900 transition-all"
                     :class="{'border-rose-400': errors.name}" />
            </div>
            <span v-if="errors.name" class="text-[10px] font-bold text-rose-500 ml-2 uppercase">{{ errors.name }}</span>
          </div>

          <div class="space-y-1">
            <div class="relative group">
              <i class="ph ph-envelope-simple absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 text-lg group-focus-within:text-slate-900 transition-colors"></i>
              <input v-model="form.email" type="email" placeholder="E-mail"
                     class="w-full pl-12 pr-4 py-4 rounded-2xl bg-slate-50 border border-slate-200 text-sm font-bold outline-none focus:bg-white focus:border-slate-900 transition-all"
                     :class="{'border-rose-400': errors.email}" />
            </div>
            <span v-if="errors.email" class="text-[10px] font-bold text-rose-500 ml-2 uppercase">{{ errors.email }}</span>
          </div>

          <div class="space-y-1">
            <div class="relative group">
              <i class="ph ph-lock-key absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 text-lg group-focus-within:text-slate-900 transition-colors"></i>
              <input v-model="form.password" :type="showPassword ? 'text' : 'password'" placeholder="Senha"
                     class="w-full pl-12 pr-12 py-4 rounded-2xl bg-slate-50 border border-slate-200 text-sm font-bold outline-none focus:bg-white focus:border-slate-900 transition-all"
                     :class="{'border-rose-400': errors.password}" />
              <button type="button" @click="showPassword = !showPassword" class="absolute right-4 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-900 transition-colors">
                <i class="ph-bold" :class="showPassword ? 'ph-eye-closed' : 'ph-eye'"></i>
              </button>
            </div>
            <span v-if="errors.password" class="text-[10px] font-bold text-rose-500 ml-2 uppercase">{{ errors.password }}</span>
          </div>

          <div class="space-y-1">
            <div class="relative group">
              <i class="ph ph-shield-check absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 text-lg group-focus-within:text-slate-900 transition-colors"></i>
              <input v-model="form.confirmPassword" :type="showPassword ? 'text' : 'password'" placeholder="Confirmar Senha"
                     class="w-full pl-12 pr-4 py-4 rounded-2xl bg-slate-50 border border-slate-200 text-sm font-bold outline-none focus:bg-white focus:border-slate-900 transition-all"
                     :class="{'border-rose-400': errors.confirmPassword}" />
            </div>
            <span v-if="errors.confirmPassword" class="text-[10px] font-bold text-rose-500 ml-2 uppercase">{{ errors.confirmPassword }}</span>
          </div>

          <div class="pt-4">
            <LoginButton
                :is-loading="isLoading"
                text="Criar minha conta"
                class="bg-slate-900 hover:bg-slate-800"
            />
          </div>
        </form>

        <div class="mt-8">
          <ActionText
              text="Já faz parte do Lumen?"
              action-label="Entrar na conta"
              @action="router.push('/login')"
          />
        </div>
      </div>
    </section>
  </MobileLayoutBase>
</template>