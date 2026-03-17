<script setup lang="ts">
import { ref } from "vue";
import MobileLayoutBase from "../Components/Base/MobileLayoutBase.vue";
const modalConfig = ref({ show: false, title: '', message: '', type: 'success' });

const handleSucesso = () => {
  modalConfig.value = {
    show: true,
    title: 'Tudo certo!',
    message: 'Seu produto foi salvo com sucesso.',
    type: 'success'
  };
};
</script>

<template>
  <MobileLayoutBase :modal="modalConfig" @close-modal="modalConfig.show = false">
    <button @click="handleSucesso">Salvar</button>
  </MobileLayoutBase>
</template>