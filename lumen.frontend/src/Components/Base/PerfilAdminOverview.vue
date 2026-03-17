<script setup lang="ts">
import StoreIdentityWidget from "./StoreIdentityWidget.vue";

const nomeLoja = defineModel<string>('nomeLoja')
const bio = defineModel<string>('bio')
const whatsapp = defineModel<string>('whatsapp')
const instagram = defineModel<string>('instagram')
const chavepix = defineModel<string>('chavepix')
const slug = defineModel<string>('slug')

defineEmits(['open-business', 'open-metrics'])
</script>

<template>
  <div class="space-y-6">
    <StoreIdentityWidget
        v-model:nomeLoja="nomeLoja"
        v-model:bio="bio"
        v-model:whatsapp="whatsapp"
        v-model:chavepix="chavepix"
        v-model:instagram="instagram"
        v-model:slug="slug"
    />
  </div>
</template>