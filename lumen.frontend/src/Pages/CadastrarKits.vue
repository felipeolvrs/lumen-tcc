<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { type Produto, produtoService } from "../Services/ProdutoService.ts";
import { colecaoService } from "../Services/ColecaoService.ts";
import ProductHeader from "../Components/Base/ProductHeader.vue";
import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";
import PhotosGalleryEditar from "../Components/PhotosGalleryEditar.vue";

const router = useRouter();
const route = useRoute();

const colecaoId = route.params.id as string;
const isEditMode = computed(() => !!colecaoId);

const DEFAULT_IMAGE = "https://placehold.co/400x400/f1f5f9/94a3b8?text=Sem+Imagem";

const products = ref<Produto[]>([]);
const search = ref("");
const isLoading = ref(false);
const isSaving = ref(false);

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
  nome: '',
  descricao: '',
  capa: [] as string[],
  produtosSelecionados: [] as string[]
});

onMounted(async () => {
  isLoading.value = true;
  try {
    const [prodsRes, colRes] = await Promise.all([
      produtoService.listar(),
      isEditMode.value ? colecaoService.obterPorId(colecaoId) : Promise.resolve({ data: null })
    ]);

    if (prodsRes.data) products.value = prodsRes.data;

    if (colRes?.data) {
      const col = colRes.data;
      form.value = {
        nome: col.nome,
        descricao: col.descricao || '',
        capa: col.urlCapa ? [col.urlCapa] : [],
        produtosSelecionados: col.produtos?.map((p: any) => p.id) || []
      };
    }
  }
  catch (error) {
    notificar("Erro", "Falha ao carregar catálogo de produtos.", "error");
  } finally {
    isLoading.value = false;
  }
});

const filteredProducts = computed(() => {
  if (!search.value) return products.value;
  const s = search.value.toLowerCase();
  return products.value.filter(p => p.nome.toLowerCase().includes(s));
});

const toggleProduct = (id: string) => {
  const index = form.value.produtosSelecionados.indexOf(id);
  if (index > -1) {
    form.value.produtosSelecionados.splice(index, 1);
  }
  else {
    form.value.produtosSelecionados = [...form.value.produtosSelecionados, id];
  }
};

const handleSave = async () => {
  if (!form.value.nome || form.value.produtosSelecionados.length === 0) {
    notificar("Campos Incompletos", "Dê um nome ao kit e selecione pelo menos 1 produto.", "error");
    return;
  }

  isSaving.value = true;
  try {
    const payload = {
      id: isEditMode.value ? colecaoId : undefined,
      nome: form.value.nome,
      descricao: form.value.descricao,
      urlCapa: form.value.capa[0] || "",
      produtosIds: form.value.produtosSelecionados,
      publica: true
    };

    if (isEditMode.value) {
      await colecaoService.atualizar(colecaoId, payload);
      notificar("Sucesso", "Kit atualizado com sucesso!", "success");
    } else {
      await colecaoService.criar(payload);
      notificar("Sucesso", "Kit criado com sucesso!", "success");
    }

    setTimeout(() => {
      router.push('/colecao');
    }, 1500);
  }
  catch (error) {
    notificar("Erro ao Salvar", "Ocorreu um problema ao processar o kit.", "error");
  } finally {
    isSaving.value = false;
  }
};

const handleDelete = async () => {
  if (!confirm("Tem certeza que deseja excluir este Kit permanentemente?")) return;

  try {
    await colecaoService.excluir(colecaoId);
    notificar("Removido", "O Kit foi excluído com sucesso.", "success");
    setTimeout(() => {
      router.push('/colecao');
    }, 1500);
  } catch (error) {
    notificar("Erro", "Falha ao excluir o kit selecionado.", "error");
  }
};
</script>

<template>
  <MobileLayoutBase :modal="modalConfig" @close-modal="modalConfig.show = false">
    <ProductHeader
        :title="isEditMode ? 'Ajustar Kit' : 'Novo Kit'"
        :show-title="true"
    />

    <div v-if="isLoading" class="flex flex-col items-center justify-center py-40 space-y-4">
      <i class="ph-bold ph-spinner animate-spin text-4xl text-slate-900"></i>
      <p class="text-[10px] font-black text-slate-400 uppercase tracking-widest">Carregando catálogo...</p>
    </div>

    <div v-else class="p-6 pb-44 space-y-8">
      <section class="space-y-4">
        <div>
          <label class="block text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1 mb-2">Imagem de Capa</label>
          <PhotosGalleryEditar v-model="form.capa" />
        </div>

        <div class="space-y-4">
          <div>
            <label class="block text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1 mb-2">Título do Kit</label>
            <input
                v-model="form.nome"
                type="text"
                placeholder="Ex: Setup Gamer White"
                class="w-full bg-slate-50 border border-slate-200 rounded-2xl px-5 py-4 text-sm font-bold outline-none focus:border-slate-900 focus:bg-white transition-all shadow-sm"
            />
          </div>

          <div>
            <label class="block text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1 mb-2">Descrição (Opcional)</label>
            <textarea
                v-model="form.descricao"
                rows="3"
                placeholder="Descreva o conceito desta coleção..."
                class="w-full bg-slate-50 border border-slate-200 rounded-2xl px-5 py-4 text-sm font-bold outline-none focus:border-slate-900 focus:bg-white transition-all resize-none shadow-sm"
            ></textarea>
          </div>
        </div>
      </section>

      <hr class="border-slate-100">

      <section class="space-y-4">
        <div class="flex justify-between items-center">
          <label class="block text-[10px] font-black text-slate-400 uppercase tracking-widest ml-1">Itens do Kit</label>
          <span class="text-[10px] font-black text-slate-900 bg-slate-100 px-3 py-1.5 rounded-full">
            {{ form.produtosSelecionados.length }} selecionados
          </span>
        </div>

        <div class="relative flex items-center bg-slate-100 rounded-2xl px-4 py-3 focus-within:ring-2 focus-within:ring-slate-900/10 transition-all">
          <i class="ph ph-magnifying-glass text-slate-400"></i>
          <input
              v-model="search"
              type="text"
              placeholder="Pesquisar em seu catálogo..."
              class="bg-transparent border-none w-full ml-3 text-xs font-bold outline-none"
          />
        </div>

        <div class="grid grid-cols-1 gap-3 max-h-[400px] overflow-y-auto pr-2">
          <div
              v-for="product in filteredProducts"
              :key="product.id"
              @click="toggleProduct(product.id!)"
              class="flex items-center p-3 rounded-[24px] border-2 transition-all cursor-pointer group"
              :class="form.produtosSelecionados.includes(product.id!)
                ? 'border-slate-900 bg-slate-50/50'
                : 'border-slate-100 bg-white active:scale-[0.98]'"
          >
            <div class="relative w-16 h-16 rounded-2xl overflow-hidden flex-shrink-0 bg-slate-100">
              <img :src="product.urlImagem || DEFAULT_IMAGE" class="w-full h-full object-cover"  alt=""/>
              <div v-if="form.produtosSelecionados.includes(product.id!)" class="absolute inset-0 bg-slate-900/40 flex items-center justify-center animate-in zoom-in-50">
                <i class="ph-bold ph-check text-white text-xl"></i>
              </div>
            </div>

            <div class="flex-1 ml-4 min-w-0">
              <p class="text-sm font-black text-slate-800 truncate leading-tight">{{ product.nome }}</p>
              <p class="text-xs font-bold text-slate-400 mt-1">R$ {{ product.preco }}</p>
            </div>

            <div
                class="w-6 h-6 rounded-full border-2 flex items-center justify-center transition-all"
                :class="form.produtosSelecionados.includes(product.id!)
                  ? 'bg-slate-900 border-slate-900 scale-110'
                  : 'border-slate-200 group-hover:border-slate-300'"
            >
              <i v-if="form.produtosSelecionados.includes(product.id!)" class="ph-bold ph-check text-[10px] text-white"></i>
            </div>
          </div>
        </div>
      </section>

      <button
          v-if="isEditMode"
          @click="handleDelete"
          class="w-full py-4 text-rose-500 font-bold text-xs flex items-center justify-center gap-2 hover:bg-rose-50 rounded-2xl transition-colors mt-8"
      >
        <i class="ph-bold ph-trash"></i>
        Excluir este Kit permanentemente
      </button>
    </div>

    <template #footer>
      <div class="p-6 bg-white/90 backdrop-blur-xl border-t border-slate-100">
        <button
            @click="handleSave"
            :disabled="isSaving"
            class="w-full bg-slate-900 text-white py-4 rounded-2xl font-black text-base shadow-xl active:scale-95 transition-all disabled:opacity-50"
        >
          <span v-if="!isSaving">
            {{ isEditMode ? 'Salvar Alterações' : 'Criar Kit Agora' }}
          </span>
          <i v-else class="ph-bold ph-spinner animate-spin text-xl"></i>
        </button>
      </div>
    </template>
  </MobileLayoutBase>
</template>