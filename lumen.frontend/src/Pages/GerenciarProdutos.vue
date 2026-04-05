<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { useRouter } from "vue-router";
import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";
import ConfigHeader from "../Components/Base/ConfigHeader.vue";
import ButtonSheetBase from "../Components/Base/ButtonSheetBase.vue";
import ProductContent from "../Components/ProductContent.vue";
import { produtoService, type Produto } from "../Services/ProdutoService.ts";

const router = useRouter();
const DEFAULT_IMAGE = "https://placehold.co/400x400/f1f5f9/94a3b8?text=Sem+Imagem";

const produtos = ref<Produto[]>([]);
const isDataLoading = ref(true);
const isProductOpen = ref(false);
const isStatusLoading = ref(false);
const selectedProduct = ref<Produto | null>(null);

const modalConfig = ref({
  show: false,
  title: '',
  message: '',
  type: 'error' as 'success' | 'error'
});

const notificar = (title: string, message: string, type: 'success' | 'error' = 'error') => {
  modalConfig.value = { show: true, title, message, type };
};

const fetchProdutos = async () => {
  isDataLoading.value = true;
  try {
    const response = await produtoService.listar();
    if (response.data) produtos.value = response.data;
  } catch (error) {
    notificar("Erro", "Falha ao carregar produtos.");
  } finally {
    isDataLoading.value = false;
  }
};

const handleToggleStatus = async () => {
  if (!selectedProduct.value?.id) return;

  const id = selectedProduct.value.id;
  const statusAtual = selectedProduct.value.ativo;
  isStatusLoading.value = true;

  try
  {
    const response = statusAtual
        ? await produtoService.inativar(id)
        : await produtoService.ativar(id);

    if (response.success)
    {
      selectedProduct.value.ativo = !statusAtual;
      const index = produtos.value.findIndex(p => p.id === id);

      if (index !== -1)
      {
        const produto = produtos.value[index];

        if (produto)
          produto.ativo = !statusAtual;

      }
      notificar("Sucesso", "Status atualizado.", "success");
    }
  }
  catch (error)
  {
    notificar("Erro", "Falha na operação.");
  }
  finally
  {
    isStatusLoading.value = false;
  }
};

onMounted(fetchProdutos);

const processedProdutos = computed(() => {
  return produtos.value.map(p => ({
    ...p,
    urlImagem: p.urlImagem || DEFAULT_IMAGE
  }));
});

const handleOpenProduct = (produto: Produto) => {
  selectedProduct.value = produto;
  isProductOpen.value = true;
};

const handleVerDetalhes = (id?: string) => {
  isProductOpen.value = false;
  if (id) router.push(`/produto/detalhes/${id}`);
};
</script>

<template>
  <MobileLayoutBase :modal="modalConfig" @close-modal="modalConfig.show = false">
    <ConfigHeader title="Produtos" />

    <div class="px-4 pb-4">
      <div v-if="isDataLoading" class="flex justify-center py-8">
        <p class="text-sm text-slate-400 font-medium">Carregando...</p>
      </div>

      <div v-else class="flex flex-col gap-3 mt-2">
        <div
            v-for="item in processedProdutos"
            :key="item.id"
            @click="handleOpenProduct(item)"
            class="flex items-center p-3 bg-white border border-slate-100 rounded-3xl shadow-sm cursor-pointer hover:border-slate-200 transition-colors"
        >
          <div class="w-16 h-16 rounded-2xl overflow-hidden bg-slate-100 flex-shrink-0">
            <img :src="item.urlImagem" class="w-full h-full object-cover" />
          </div>
          <div class="flex-1 ml-4">
            <div class="flex items-center gap-2">
              <h3 class="text-slate-900 font-bold text-sm line-clamp-1">{{ item.nome }}</h3>
              <span v-if="!item.ativo" class="text-[9px] bg-red-100 text-red-600 px-1.5 py-0.5 rounded-md font-bold uppercase">Inativo</span>
            </div>
            <p class="text-[10px] font-bold text-slate-400 mt-1 uppercase tracking-wider">Produto</p>
          </div>
          <div class="pr-2">
            <i class="ph-bold ph-caret-right text-slate-300"></i>
          </div>
        </div>
      </div>
    </div>

    <ButtonSheetBase :is-open="isProductOpen" @close="isProductOpen = false">
      <div class="flex flex-col">
        <div class="px-6 py-4">
          <div class="h-[1px] w-full bg-slate-100 mb-6"></div>
          <button
              @click="handleToggleStatus"
              :disabled="isStatusLoading"
              class="w-full py-4 rounded-2xl flex items-center justify-center gap-3 transition-all duration-300 font-bold text-sm active:scale-95 disabled:opacity-50 shadow-sm"
              :class="selectedProduct?.ativo
              ? 'bg-rose-50 text-rose-600 border border-rose-100 hover:bg-rose-100'
              : 'bg-emerald-500 text-white hover:bg-emerald-600 shadow-emerald-100 shadow-lg'"
          >
            <i v-if="isStatusLoading" class="ph-bold ph-circle-notch animate-spin text-lg"></i>
            <i v-else :class="selectedProduct?.ativo ? 'ph-bold ph-power' : 'ph-bold ph-check-circle'" class="text-lg"></i>
            <span>{{ selectedProduct?.ativo ? 'Desativar Produto' : 'Ativar Produto' }}</span>
          </button>
          <p class="text-center text-[11px] text-slate-400 mt-4 leading-relaxed px-4">
            {{ selectedProduct?.ativo ? 'Ao desativar, este item ficará oculto na vitrine.' : 'Ao ativar, o produto voltará a ficar disponível.' }}
          </p>
        </div>
      </div>
    </ButtonSheetBase>
  </MobileLayoutBase>
</template>