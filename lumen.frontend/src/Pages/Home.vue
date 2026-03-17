<script setup lang="ts">
import { ref, onMounted, computed } from "vue";
import { useRouter } from "vue-router";

import HeaderProfile from "../Components/HeaderProfile.vue";
import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";
import NavbarBar from "../Components/Base/NavbarBar.vue";
import MainProfile from "../Components/MainProfile.vue";
import ButtonSheetBase from "../Components/Base/ButtonSheetBase.vue";
import CartContent from "../Components/Base/CartContent.vue";
import ProductContent from "../Components/ProductContent.vue";
import AddContent from "../Components/AddContent.vue";
import DrawerKit from "../Components/DrawerKit.vue";

import { produtoService, type Produto } from "../Services/ProdutoService.ts";
import { colecaoService, type ColecaoResponse } from "../Services/ColecaoService.ts";
import { usuarioService, type UsuarioPerfil } from "../Services/UsuarioService.ts";

const router = useRouter();

const DEFAULT_IMAGE = "https://placehold.co/400x400/f1f5f9/94a3b8?text=Sem+Imagem";

const produtos = ref<Produto[]>([]);
const colecoes = ref<ColecaoResponse[]>([]);
const userProfile = ref<UsuarioPerfil | null>(null);
const isDataLoading = ref(true);
const selectedProduct = ref<Produto | null>(null);
const selectedColecao = ref<ColecaoResponse | null>(null);

const modalConfig = ref({
  show: false,
  title: '',
  message: '',
  type: 'error' as 'success' | 'error'
});

const notificar = (title: string, message: string, type: 'success' | 'error' = 'error') => {
  modalConfig.value = { show: true, title, message, type };
};

const processedProdutos = computed(() => {
  return produtos.value.map(p => ({
    ...p,
    urlImagem: p.urlImagem || DEFAULT_IMAGE
  }));
});

const processedColecoes = computed(() => {
  return colecoes.value.map(c => ({
    ...c,
    urlCapa: c.urlCapa || DEFAULT_IMAGE
  }));
});

const fetchHomeData = async () =>
{
  isDataLoading.value = true;
  try
  {
    const [prodRes, colRes, userRes] = await Promise.all([
      produtoService.listar(),
      colecaoService.listar(),
      usuarioService.obterPerfil()
    ]);

    if (prodRes.data) produtos.value = prodRes.data;
    if (colRes.data) colecoes.value = colRes.data;

    if (userRes.data)
    {
      userProfile.value = {
        ...userRes.data,
        username: (userRes.data as any).slug || userRes.data.username || '',
        fotoUrl: (userRes.data as any).urlLogo || userRes.data.fotoUrl || '',
      };
    }
  } catch (error) {
    notificar("Erro de Carregamento", "Não conseguimos sincronizar os dados da sua vitrine.", "error");
  } finally {
    isDataLoading.value = false;
  }
};

onMounted(fetchHomeData);

const isCartOpen = ref(false);
const isProductOpen = ref(false);
const isItemsColecaoOpen = ref(false);
const isAddOpen = ref(false);

const closeAllSheets = () => {
  isCartOpen.value = false;
  isProductOpen.value = false;
  isItemsColecaoOpen.value = false;
  isAddOpen.value = false;
};

const getSlug = () => userProfile.value?.slug || userProfile.value?.username || 'loja';

const handleHome = () => { closeAllSheets(); router.push('/'); };
const handlePerfil = () => { closeAllSheets(); router.push(`/perfil`); };
const handleCheckout = () => { closeAllSheets(); router.push(`/${getSlug()}/checkout`); };
const handleAllColecoes = () => { closeAllSheets(); router.push(`/colecao`); };
const handleCadastroProduto = () => { closeAllSheets(); router.push(`/produto/cadastro/`); };
const handleCadastroColecao = () => { closeAllSheets(); router.push(`/colecao/cadastro/`); };

const handleOpenProduct = (produto: Produto) => {
  selectedProduct.value = produto;
  isProductOpen.value = true;
};

const handleOpenColecao = (colecao: ColecaoResponse) => {
  selectedColecao.value = colecao;
  isItemsColecaoOpen.value = true;
};

const handleVerDetalhes = (id?: string) => {
  closeAllSheets();
  if (id) router.push(`/produto/detalhes/${id}`);
};
</script>

<template>
  <MobileLayoutBase :modal="modalConfig" @close-modal="modalConfig.show = false">
    <HeaderProfile
        v-if="userProfile"
        :name="userProfile.nomeLoja || userProfile.nome || 'Usuário Lumen'"
        :handle="`@${userProfile.username || 'usuario'}`"
        :avatar="userProfile.fotoUrl || DEFAULT_IMAGE"
        :bio="userProfile.bio || 'Bem-vindo à sua vitrine!'"
        :is-verified="true"
        :is-online="userProfile.ativo"
    />

    <MainProfile
        :produtos="processedProdutos"
        :colecoes="processedColecoes"
        :is-loading="isDataLoading"
        @open-product="handleOpenProduct"
        @open-items-colecao="handleOpenColecao"
        @open-colecao="handleAllColecoes"
    />

    <template #footer>
      <NavbarBar
          :user-avatar="userProfile?.fotoUrl"
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

    <ButtonSheetBase :is-open="isProductOpen" @close="isProductOpen = false">
      <ProductContent
          :produto="selectedProduct"
          @close-drawer="isProductOpen = false"
          @open-ver-detalhes="handleVerDetalhes(selectedProduct?.id)"
      />
    </ButtonSheetBase>

    <ButtonSheetBase :is-open="isAddOpen" @close="isAddOpen = false">
      <AddContent title="O que vamos criar hoje?" @open-cadastro-produto="handleCadastroProduto" @open-cadastro-colecao="handleCadastroColecao"/>
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