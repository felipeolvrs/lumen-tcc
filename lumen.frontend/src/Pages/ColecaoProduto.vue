<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";

import { colecaoService, type ColecaoResponse } from "../Services/ColecaoService.ts";
import { usuarioService, type UsuarioPerfil } from "../Services/UsuarioService.ts";

import ProductHeader from "../Components/Base/ProductHeader.vue";
import NavbarBar from "../Components/Base/NavbarBar.vue";
import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";
import ButtonSheetBase from "../Components/Base/ButtonSheetBase.vue";
import CartContent from "../Components/Base/CartContent.vue";
import AddContent from "../Components/AddContent.vue";
import DrawerKit from "../Components/DrawerKit.vue";

const router = useRouter();

const isLoading = ref(true);
const viewMode = ref<'grid' | 'list'>('grid');

const isCartOpen = ref(false);
const isAddOpen = ref(false);
const isItemsColecaoOpen = ref(false);

const DEFAULT_COVER = "https://placehold.co/400x600/f1f5f9/94a3b8?text=Sem+Capa";

const modalConfig = ref({
  show: false,
  title: '',
  message: '',
  type: 'error' as 'success' | 'error'
});

const notificar = (title: string, message: string, type: 'success' | 'error' = 'error') => {
  modalConfig.value = { show: true, title, message, type };
};

const colecoes = ref<ColecaoResponse[]>([]);
const selectedColecao = ref<ColecaoResponse | null>(null);
const userProfile = ref<Partial<UsuarioPerfil>>({ fotoUrl: '' });

const fetchInitialData = async () =>
{
  isLoading.value = true;
  try
  {
    const [resColecoes, resUser] = await Promise.all([
      colecaoService.listar(),
      usuarioService.obterPerfil()
    ]);

    if (resColecoes.data) {
      colecoes.value = resColecoes.data;
    } else if (Array.isArray(resColecoes)) {
      colecoes.value = resColecoes;
    }

    if (resUser.data)
    {
      userProfile.value = {
        ...resUser.data,
        fotoUrl: (resUser.data as any).urlLogo || resUser.data.fotoUrl || ''
      };
    }
  } catch (error) {
    notificar("Erro de Conexão", "Não conseguimos buscar seus kits agora.", "error");
  } finally {
    isLoading.value = false;
  }
};

onMounted(fetchInitialData);

const closeAllSheets = () => {
  isCartOpen.value = false;
  isAddOpen.value = false;
  isItemsColecaoOpen.value = false;
};

const handleHome = () => {
  closeAllSheets();
  router.push('/');
};

const handlePerfil = () => {
  closeAllSheets();
  router.push('/perfil');
};

const handleAllColecoes = () => {
  closeAllSheets();
  if (router.currentRoute.value.path === '/colecao') {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    return;
  }
  router.push('/colecao');
};

const handleCadastroProduto = () => {
  closeAllSheets();
  router.push('/produto/cadastro/');
};

const handleCadastroColecao = () => {
  closeAllSheets();
  router.push('/colecao/cadastro/');
};

const handleCheckout = () => {
  closeAllSheets();
  router.push('/checkout');
};

const handleVerDetalhes = (id: string) => {
  closeAllSheets();
  router.push(`/produto/detalhes/${id}`);
};

const abrirKit = (colecao: ColecaoResponse) => {
  selectedColecao.value = colecao;
  isItemsColecaoOpen.value = true;
};
</script>

<template>
  <MobileLayoutBase :modal="modalConfig" @close-modal="modalConfig.show = false">
    <ProductHeader title="Meus Kits" :show-title="true">
      <template #action>
        <div class="flex bg-slate-100 rounded-lg p-1 gap-1">
          <button
              @click="viewMode = 'grid'"
              class="p-1.5 rounded-md transition-all"
              :class="viewMode === 'grid' ? 'bg-white text-slate-900 shadow-sm' : 'text-slate-400'"
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-4 h-4"><path stroke-linecap="round" stroke-linejoin="round" d="M3.75 6A2.25 2.25 0 016 3.75h2.25A2.25 2.25 0 0110.5 6v2.25a2.25 2.25 0 01-2.25 2.25H6a2.25 2.25 0 01-2.25-2.25V6zM3.75 15.75A2.25 2.25 0 016 13.5h2.25a2.25 2.25 0 012.25 2.25V18a2.25 2.25 0 01-2.25 2.25H6A2.25 2.25 0 013.75 18v-2.25zM13.5 6a2.25 2.25 0 012.25-2.25H18A2.25 2.25 0 0120.25 6v2.25A2.25 2.25 0 0118 10.5h-2.25a2.25 2.25 0 01-2.25-2.25V6zM13.5 15.75a2.25 2.25 0 012.25-2.25H18A2.25 2.25 0 0120.25 6v2.25A2.25 2.25 0 0113.5 18v-2.25z" /></svg>
          </button>
          <button
              @click="viewMode = 'list'"
              class="p-1.5 rounded-md transition-all"
              :class="viewMode === 'list' ? 'bg-white text-slate-900 shadow-sm' : 'text-slate-400'"
          >
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-4 h-4"><path stroke-linecap="round" stroke-linejoin="round" d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5" /></svg>
          </button>
        </div>
      </template>
    </ProductHeader>

    <div class="px-4 py-6 pb-32">
      <div v-if="isLoading" class="flex justify-center py-20">
        <i class="ph-bold ph-spinner animate-spin text-2xl text-slate-900"></i>
      </div>

      <div v-else-if="colecoes.length === 0" class="text-center py-20 space-y-4">
        <div class="w-20 h-20 bg-slate-50 rounded-full flex items-center justify-center mx-auto">
          <i class="ph-fill ph-package text-3xl text-slate-200"></i>
        </div>
        <div>
          <h3 class="font-bold text-slate-800">Nenhum kit encontrado</h3>
          <p class="text-sm text-slate-400 mt-1">Crie sua primeira coleção de produtos</p>
        </div>
        <button @click="isAddOpen = true" class="text-slate-900 font-bold text-sm underline decoration-slate-200 underline-offset-4">
          Criar Novo Kit
        </button>
      </div>

      <div v-else-if="viewMode === 'grid'" class="grid grid-cols-2 gap-4">
        <div
            v-for="item in colecoes"
            :key="item.id"
            @click="abrirKit(item)"
            class="group relative aspect-[3/4] rounded-3xl overflow-hidden shadow-sm cursor-pointer bg-slate-100"
        >
          <img
              :src="item.urlCapa || DEFAULT_COVER"
              class="w-full h-full object-cover group-hover:scale-105 transition duration-500"
              alt="Capa da Coleção"
          />
          <div class="absolute inset-0 bg-gradient-to-t from-slate-900 via-transparent to-transparent opacity-80"></div>
          <div class="absolute bottom-0 p-4 text-white">
            <h3 class="font-bold text-sm leading-tight mb-1">{{ item.nome }}</h3>
            <p class="text-[10px] text-slate-300 font-medium">
              {{ item.produtos ? item.produtos.length : 0 }} produtos
            </p>
          </div>
        </div>
      </div>

      <div v-else class="flex flex-col gap-3">
        <div
            v-for="item in colecoes"
            :key="item.id"
            @click="abrirKit(item)"
            class="flex items-center p-3 bg-white border border-slate-100 rounded-3xl shadow-sm cursor-pointer hover:border-slate-200 transition-colors"
        >
          <div class="w-16 h-16 rounded-2xl overflow-hidden bg-slate-100 flex-shrink-0">
            <img
                :src="item.urlCapa || DEFAULT_COVER"
                class="w-full h-full object-cover"
                alt=""
            />
          </div>
          <div class="flex-1 ml-4">
            <h3 class="text-slate-900 font-bold text-sm">{{ item.nome }}</h3>
            <p class="text-[10px] font-bold text-slate-400 mt-1 uppercase tracking-wider">
              {{ item.produtos ? item.produtos.length : 0 }} itens
            </p>
          </div>
          <div class="pr-2">
            <i class="ph-bold ph-caret-right text-slate-300"></i>
          </div>
        </div>
      </div>
    </div>

    <template #footer>
      <NavbarBar
          :user-avatar="userProfile.fotoUrl"
          @go-home="handleHome"
          @open-cart="isCartOpen = true"
          @open-add="isAddOpen = true"
          @open-perfil="handlePerfil"
          @open-kits="handleAllColecoes"
          @close-all="closeAllSheets"
      />
    </template>

    <ButtonSheetBase :is-open="isCartOpen" @close="isCartOpen = false">
      <CartContent @go-to-checkout="handleCheckout" />
    </ButtonSheetBase>

    <ButtonSheetBase :is-open="isAddOpen" @close="isAddOpen = false">
      <AddContent title="O que vamos criar hoje?" @open-cadastro-produto="handleCadastroProduto" @open-cadastro-colecao="handleCadastroColecao" />
    </ButtonSheetBase>

    <ButtonSheetBase :is-open="isItemsColecaoOpen" @close="isItemsColecaoOpen = false">
      <DrawerKit
          :colecao="selectedColecao"
          @close-drawer="isItemsColecaoOpen = false"
          @ver-detalhes="handleVerDetalhes"
      />
    </ButtonSheetBase>

  </MobileLayoutBase>
</template>