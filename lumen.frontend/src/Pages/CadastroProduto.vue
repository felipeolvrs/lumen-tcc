<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { type Produto, produtoService } from "../Services/ProdutoService.ts";

import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";
import ProductHeader from "../Components/Base/ProductHeader.vue";
import PhotosGalleryEditar from "../Components/PhotosGalleryEditar.vue";
import TipoSeletor from "../Components/TipoSeletor.vue";
import AffiliateInputSection from "../Components/AffiliateInputSection.vue";
import ProductBasicInfo from "../Components/ProductBasicInfo.vue";

const route = useRoute();
const router = useRouter();

const productId = route.params.id as string;
const isEditMode = computed(() => !!productId);
const isLoading = ref(false);

const DEFAULT_IMAGE = "https://placehold.co/400x400/f1f5f9/94a3b8?text=Sem+Imagem";

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
  description: '',
  price: 0,
  oldprice: 0,
  photos: [] as string[],
  link: '',
  category: 'own',
  review: '',
  platform: 'shopee'
});

const loadProduto = async () => {
  if (!isEditMode.value) return;

  isLoading.value = true;
  try {
    const { data } = await produtoService.obterPorId(productId);
    if (data) {
      form.value = {
        name: data.nome,
        description: data.descricao,
        price: data.preco,
        oldprice: data.precoAntigo || 0,
        photos: data.urlImagem ? [data.urlImagem] : [],
        category: data.categoria || 'own',
        link: data.linkAfiliado || '',
        platform: data.plataforma || 'shopee',
        review: data.reviewPessoal || ''
      };
    }
  } catch (error) {
    notificar("Erro", "Não foi possível carregar os dados do produto.", "error");
  } finally {
    isLoading.value = false;
  }
};

onMounted(loadProduto);

const handleDelete = async () => {
  if (!confirm("Tem certeza que deseja excluir este produto permanentemente?")) return;

  isLoading.value = true;
  try {
    await produtoService.remover(productId);
    notificar("Removido", "O produto foi excluído da sua vitrine.", "success");

    setTimeout(() => {
      router.push('/');
    }, 1500);
  } catch (error) {
    notificar("Erro", "Falha ao tentar remover o produto.", "error");
  } finally {
    isLoading.value = false;
  }
};

const handleSave = async () => {
  if (isLoading.value) return;

  const isAffiliate = form.value.category === 'affiliate';

  if (!form.value.name || !form.value.description || form.value.price <= 0) {
    notificar("Campos Vazios", "Preencha o nome, descrição e preço do produto.", "error");
    return;
  }

  isLoading.value = true;
  try {
    const payload: Produto = {
      id: isEditMode.value ? productId : undefined,
      nome: form.value.name,
      descricao: form.value.description,
      preco: Number(form.value.price),
      precoAntigo: Number(form.value.price),
      categoria: form.value.category,
      urlImagem: form.value.photos[0] || "",
      linkAfiliado: isAffiliate ? form.value.link : undefined,
      plataforma: isAffiliate ? form.value.platform : undefined,
      reviewPessoal: form.value.review
    };

    if (isEditMode.value) {
      await produtoService.atualizar(productId, payload);
      notificar("Sucesso!", "Produto atualizado com sucesso!", "success");
    } else {
      await produtoService.criar(payload);
      notificar("Sucesso!", "Produto cadastrado na sua vitrine.", "success");
    }

    setTimeout(() => {
      router.push('/');
    }, 1500);
  } catch (error: any) {
    notificar("Erro ao Salvar", "Verifique os dados ou o tamanho da imagem.", "error");
  } finally {
    isLoading.value = false;
  }
};

const handleShare = async () => {
  if (navigator.share) {
    await navigator.share({
      title: form.value.name,
      text: form.value.description,
      url: window.location.href
    });
  }
};
</script>

<template>
  <MobileLayoutBase
      :modal="modalConfig"
      @close-modal="modalConfig.show = false"
  >
    <ProductHeader
        :title="isEditMode ? 'Editar Produto' : 'Novo Produto'"
        :show-title="true"
        @share="handleShare"
    />

    <div class="p-6 space-y-8 pb-32">
      <PhotosGalleryEditar v-model="form.photos" />

      <section class="space-y-4">
        <TipoSeletor v-model="form.category" />
      </section>

      <form @submit.prevent="handleSave" class="space-y-6">
        <div v-if="form.category === 'affiliate'" class="py-4 border-y border-slate-100">
          <AffiliateInputSection
              v-model:linkValue="form.link"
              v-model:selectedPlatformId="form.platform"
          />
        </div>

        <ProductBasicInfo
            v-model="form"
            :is-edit-mode="isEditMode"
            @delete="handleDelete"
        />

        <div class="pt-6">
          <button
              type="submit"
              :disabled="isLoading"
              class="w-full bg-slate-900 text-white py-4 rounded-2xl font-bold text-lg active:scale-[0.98] transition-all disabled:opacity-50 shadow-xl"
          >
            <span v-if="isLoading" class="flex items-center justify-center gap-2">
              <i class="ph-bold ph-spinner animate-spin"></i>
              Processando...
            </span>
            <span v-else>
              {{ isEditMode ? 'Salvar Alterações' : 'Cadastrar Produto' }}
            </span>
          </button>
        </div>
      </form>
    </div>
  </MobileLayoutBase>
</template>