<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { useRouter } from "vue-router";
import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";
import ConfigHeader from "../Components/Base/ConfigHeader.vue";
import ButtonSheetBase from "../Components/Base/ButtonSheetBase.vue";
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

  try {
    const response = statusAtual
        ? await produtoService.inativar(id)
        : await produtoService.ativar(id);

    if (response.success) {
      selectedProduct.value.ativo = !statusAtual;
      const index = produtos.value.findIndex(p => p.id === id);

      if (index !== -1)
        produtos.value[index].ativo = !statusAtual;

      isProductOpen.value = false;
      notificar("Sucesso", `Produto ${!statusAtual ? 'ativado' : 'desativado'}.`, "success");

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

const processedProdutos = computed(() => {
  return produtos.value.map(p => ({
    ...p,
    urlImagem: p.urlImagem || DEFAULT_IMAGE
  }));
});

const handleOpenProduct = (produto: Produto) => {
  selectedProduct.value = { ...produto }; // Clone para evitar mutação indesejada antes da API
  isProductOpen.value = true;
};

const handleVerDetalhes = (id: string) => {
  router.push(`/produto/detalhes/${id}`);
};

const handleEditProduto = (id: string) => {
  router.push(`/produto/editar/${id}`);
};

onMounted(fetchProdutos);
</script>

<template>
  <MobileLayoutBase :modal="modalConfig" @close-modal="modalConfig.show = false">
    <ConfigHeader title="Gerenciar Produtos" />

    <div class="px-4 pb-20">
      <div v-if="isDataLoading" class="flex flex-col items-center justify-center py-12">
        <div class="w-8 h-8 border-4 border-slate-200 border-t-indigo-500 rounded-full animate-spin"></div>
        <p class="text-xs text-slate-400 font-bold uppercase tracking-widest mt-4">Carregando...</p>
      </div>

      <div v-else class="flex flex-col gap-3 mt-4">
        <div
            v-for="item in processedProdutos"
            :key="item.id"
            @click="handleOpenProduct(item)"
            class="flex items-center p-3 bg-white border border-slate-100 rounded-3xl shadow-sm cursor-pointer hover:border-indigo-100 transition-all active:scale-[0.98]"
        >
          <div class="w-16 h-16 rounded-2xl overflow-hidden bg-slate-100 flex-shrink-0 border border-slate-50">
            <img :src="item.urlImagem" class="w-full h-full object-cover" :class="{'grayscale opacity-50': !item.ativo}" />
          </div>

          <div class="flex-1 ml-4 min-w-0">
            <div class="flex items-center gap-2">
              <h3 class="text-slate-900 font-bold text-sm truncate">{{ item.nome }}</h3>
              <span v-if="!item.ativo" class="text-[9px] bg-red-50 text-red-600 px-1.5 py-0.5 rounded-md font-black uppercase">Inativo</span>
            </div>
            <p class="text-[10px] font-bold text-slate-400 mt-1 uppercase tracking-wider">Produto</p>
          </div>

          <div class="flex items-center gap-2">
            <button
                @click.stop="handleEditProduto(item.id)"
                class="p-2.5 bg-slate-50 text-blue-900 rounded-full active:scale-90 transition-all shadow-sm hover:bg-indigo-50"
                title="Editar"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil" viewBox="0 0 16 16">
                <path d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A.5.5 0 0 1 5 12.5V12h-.5a.5.5 0 0 1-.5-.5V11h-.5a.5.5 0 0 1-.468-.325"/>
              </svg>
            </button>

            <button
                @click.stop="handleVerDetalhes(item.id)"
                class="p-2.5 bg-slate-50 text-blue-900 rounded-full shadow-sm hover:bg-slate-100 active:scale-90 transition-all"
            >
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0"/>
                <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7"/>
              </svg>
            </button>
          </div>
        </div>
      </div>
    </div>

    <ButtonSheetBase :is-open="isProductOpen" @close="isProductOpen = false">
      <div class="p-6">
        <div class="flex flex-col items-center text-center mb-6">
          <div class="w-20 h-20 rounded-3xl overflow-hidden mb-4 shadow-md">
            <img :src="selectedProduct?.urlImagem || DEFAULT_IMAGE" class="w-full h-full object-cover">
          </div>
          <h2 class="text-slate-900 font-black text-lg leading-tight">{{ selectedProduct?.nome }}</h2>
        </div>

        <button
            @click="handleToggleStatus"
            :disabled="isStatusLoading"
            class="w-full py-4 rounded-2xl flex items-center justify-center gap-3 transition-all duration-300 font-bold text-base active:scale-95 disabled:opacity-50"
            :class="selectedProduct?.ativo
              ? 'bg-rose-50 text-rose-600 border border-rose-100'
              : 'bg-emerald-500 text-white shadow-lg shadow-emerald-100'"
        >
          <i v-if="isStatusLoading" class="ph-bold ph-circle-notch animate-spin text-lg"></i>
          <i v-else :class="selectedProduct?.ativo ? 'ph-bold ph-power' : 'ph-bold ph-check-circle'" class="text-lg"></i>
          <span>{{ selectedProduct?.ativo ? 'Desativar Produto' : 'Ativar Produto' }}</span>
        </button>

        <p class="text-center text-[11px] text-slate-400 mt-4 leading-relaxed px-4">
          {{ selectedProduct?.ativo
            ? 'Ao desativar, este item ficará oculto para os clientes na vitrine.'
            : 'Ao ativar, o produto ficará visível imediatamente para compra.' }}
        </p>
      </div>
    </ButtonSheetBase>
  </MobileLayoutBase>
</template>