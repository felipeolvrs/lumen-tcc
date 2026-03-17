<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { useRoute } from "vue-router";
import { produtoService } from '../Services/ProdutoService.ts';
import { usuarioService, type UsuarioPerfil } from '../Services/UsuarioService.ts';

import PhotosGallery from "../Components/Base/PhotosGallery.vue";
import ProductHeader from "../Components/Base/ProductHeader.vue";
import ReviewCard from "../Components/Base/ReviewCard.vue";
import ProductAction from "../Components/Base/ProductAction.vue";
import ProductInfo from "../Components/Base/ProductInfo.vue";
import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";

const route = useRoute();
const product = ref<any>(null);
const loading = ref(true);
const donoData = ref<UsuarioPerfil | null>(null);

const DEFAULT_IMAGE = "https://placehold.co/600x600/f1f5f9/94a3b8?text=Produto+sem+Foto";

const modalConfig = ref({
  show: false,
  title: '',
  message: '',
  type: 'error' as 'success' | 'error'
});

const notificar = (title: string, message: string, type: 'success' | 'error' = 'error') => {
  modalConfig.value = { show: true, title, message, type };
};

const carregarDonoDoCache = () => {
  const cached = localStorage.getItem('lumen_user');
  if (cached) donoData.value = JSON.parse(cached);
};

const donoNome = computed(() => donoData.value?.nome || "Dono da Loja");

const donoFoto = computed(() => {
  if (donoData.value?.fotoUrl && donoData.value.fotoUrl.length > 10) {
    return donoData.value.fotoUrl;
  }
  return `https://ui-avatars.com/api/?name=${encodeURIComponent(donoNome.value)}&background=0f172a&color=fff&bold=true`;
});

const productImage = computed(() => {
  return product.value?.urlImagem || DEFAULT_IMAGE;
});

const fetchData = async () => {
  const id = route.params.id as string;
  loading.value = true;

  try {
    const { data: produtoData } = await produtoService.obterPorId(id);
    product.value = produtoData;

    const { data: perfil } = await usuarioService.obterPerfil();

    if (perfil) {
      const perfilFormatado = {
        ...perfil,
        fotoUrl: (perfil as any).urlLogo || perfil.fotoUrl || ''
      };

      donoData.value = perfilFormatado;
      localStorage.setItem('lumen_user', JSON.stringify(perfilFormatado));
    }
  }
  catch (err) {
    notificar("Erro de Carregamento", "Não conseguimos encontrar este produto.", "error");
  }
  finally {
    loading.value = false;
  }
};

const formattedPrice = computed(() => {
  if (!product.value?.preco) return "";
  return new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(product.value.preco);
});

const hasReviewPessoal = computed(() => {
  return product.value?.reviewPessoal && product.value.reviewPessoal.trim() !== "";
});

onMounted(() => {
  carregarDonoDoCache();
  fetchData();
});
</script>

<template>
  <MobileLayoutBase :modal="modalConfig" @close-modal="modalConfig.show = false">
    <div class="bg-white min-h-full pb-32">

      <div v-if="loading" class="p-10 text-center flex flex-col items-center gap-3">
        <i class="ph-bold ph-spinner animate-spin text-3xl text-slate-900"></i>
        <span class="text-sm font-bold text-slate-400">Carregando detalhes...</span>
      </div>

      <template v-else-if="product">
        <ProductHeader :title="product.nome" :show-title="true" />

        <PhotosGallery :images="[productImage]" />

        <div class="px-6 py-6 space-y-8">
          <ProductInfo
              :tituloProduto="product.nome"
              :descricaoProduto="product.descricao"
              :preco="formattedPrice"
          />

          <ReviewCard
              v-if="hasReviewPessoal"
              sectionTitle="Opinião do Dono"
              :userName="donoNome"
              :userImage="donoFoto"
              :description="product.reviewPessoal"
          />
        </div>

        <div class="fixed bottom-0 left-0 right-0 bg-white/80 backdrop-blur-md border-t border-slate-100 p-4 z-50 max-w-md mx-auto">
          <ProductAction :product="product" />
        </div>
      </template>

      <div v-else class="p-20 text-center text-rose-500">
        <i class="ph-bold ph-warning-circle text-5xl mb-4"></i>
        <p class="font-bold">Produto não encontrado.</p>
        <button @click="$router.back()" class="mt-4 text-slate-900 underline font-bold">Voltar</button>
      </div>
    </div>
  </MobileLayoutBase>
</template>