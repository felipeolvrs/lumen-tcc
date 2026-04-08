<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useCartStore } from "../Store/useCartStore.ts";

import { pedidoService, type Pedido } from "../Services/PedidoService.ts";
import { ferramentaService } from "../Services/FerramentasService.ts";
import { usuarioService } from "../Services/UsuarioService.ts";

import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";
import ProductHeader from "../Components/Base/ProductHeader.vue";
import CheckoutIdentification from "../Components/CheckoutIdentification.vue";
import CheckoutLogistics from "../Components/CheckoutLogistics.vue";
import CheckoutPayment from "../Components/Base/CheckoutPayment.vue";
import CheckoutFooter from "../Components/CheckoutFooter.vue";
import CheckoutSummary from "../Components/CheckoutSummary.vue";

const route = useRoute();
const router = useRouter();
const cart = useCartStore();

const loja = ref<any>(null);
const isLoading = ref(false);
const errors = ref({ nome: '', telefone: '', endereco: '' });

const modalConfig = ref({
  show: false,
  title: '',
  message: '',
  type: 'error' as 'success' | 'error'
});

const notify = (title: string, message: string, type: 'success' | 'error' = 'error') => {
  modalConfig.value = { show: true, title, message, type };
};

const userData = ref<Pedido>({
  total: 0,
  nome: '',
  telefone: '',
  email: '',
  tipoEntrega: 'pickup',
  endereco: '',
  pagamento: 'pix',
  observacao: '',
  itens: []
});

const cartItems = computed(() =>
    cart.items.value.map((item) => ({
      id: item.produto.id || '',
      nome: item.produto.nome,
      preco: item.produto.preco,
      quantidade: item.quantidade,
    }))
);

const cartTotal = computed(() => cart.total.value);

onMounted(async () => {
  const slug = route.params.slug as string;
  if (!slug) return;

  try {
    const response = await usuarioService.obterPorSlug(slug);
    if (response.data) {
      loja.value = response.data;
      if (response.data.id) {
        sessionStorage.setItem('x-tenant-id', response.data.id);
      }
    }
  } catch (error) {
    notify("Erro", "Não foi possível carregar os dados da loja.", "error");
  }
});

const isFormValid = () => {
  errors.value = { nome: '', telefone: '', endereco: '' };
  let valid = true;

  if (!userData.value.nome.trim()) {
    errors.value.nome = "Nome é obrigatório";
    valid = false;
  }
  if (!userData.value.telefone.trim()) {
    errors.value.telefone = "Telefone é obrigatório";
    valid = false;
  }
  if (userData.value.tipoEntrega === 'delivery' && !userData.value.endereco.trim()) {
    errors.value.endereco = "Endereço necessário para entrega";
    valid = false;
  }

  return valid;
};

const finalizeOrder = async () => {
  if (!isFormValid()) return;

  if (cartItems.value.length === 0) {
    notify("Carrinho Vazio", "Adicione alguns produtos antes de finalizar seu pedido.", "error");
    return;
  }

  const numeroDaLoja = loja.value?.whatsApp;
  if (!numeroDaLoja) {
    notify("Ops!", "O contato da loja não foi encontrado. Tente atualizar a página.", "error");
    return;
  }

  isLoading.value = true;

  try {
    const payload = {
      ...userData.value,
      itens: cartItems.value
    };

    await pedidoService.finalizar(payload);

    const whatsPayload = {
      ...payload,
      total: cartTotal.value,
      nomeLoja: loja.value?.nomeLoja ?? 'Lumen Shop',
      numeroLoja: numeroDaLoja,
      chavePixLoja: loja.value?.chavePix ?? ''
    };

    const response = await ferramentaService.gerarLinkCheckout(whatsPayload);
    const linkParaAbrir = response.linkWhatsapp;

    if (linkParaAbrir) {
      cart.clearCart();
      window.open(linkParaAbrir, '_blank');
      router.push(`/${route.params.slug}`);
    } else {
      throw new Error();
    }
  } catch (error) {
    notify("Erro no Pedido", "Não conseguimos processar seu pedido agora. Tente novamente.", "error");
  } finally {
    isLoading.value = false;
    router.push(`/`);
  }
};
</script>

<template>
  <MobileLayoutBase
      :modal="modalConfig"
      @close-modal="modalConfig.show = false"
  >
    <ProductHeader title="Finalizar Pedido" :show-title="true" :show-share="false" />

    <div v-if="cartItems.length === 0" class="p-10 text-center flex flex-col items-center justify-center min-h-[70vh]">
      <div class="w-20 h-20 bg-slate-50 rounded-full flex items-center justify-center mb-4">
        <i class="ph-bold ph-shopping-cart text-3xl text-slate-300"></i>
      </div>
      <h3 class="text-lg font-bold text-slate-900">Carrinho vazio</h3>
      <p class="text-slate-500 text-sm mb-6">Você ainda não adicionou produtos.</p>
      <button @click="router.back()" class="bg-slate-900 text-white px-8 py-4 rounded-2xl font-bold shadow-lg">
        Voltar para a loja
      </button>
    </div>

    <form v-else id="checkout-form" @submit.prevent="finalizeOrder" class="p-6 pb-40 space-y-10">
      <CheckoutIdentification v-model="userData" :errors="errors" />
      <CheckoutLogistics v-model="userData" />
      <CheckoutPayment v-model="userData" />
      <CheckoutSummary :itens="cartItems" :total="cartTotal" />
    </form>

    <template #footer v-if="cartItems.length > 0">
      <CheckoutFooter :is-loading="isLoading" />
    </template>
  </MobileLayoutBase>
</template>

<style scoped>
.slide-in-right {
  animation: slideIn 0.3s ease-out;
}

@keyframes slideIn {
  from { transform: translateX(100%); }
  to { transform: translateX(0); }
}
</style>