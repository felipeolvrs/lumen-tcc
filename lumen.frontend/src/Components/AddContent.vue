<script setup lang="ts">
import ActionCard from "./ActionCard.vue";

interface AddContentProps {
  title: string
}

const props = withDefaults(defineProps<AddContentProps>(), {
  title: ""
})

  const emit = defineEmits(['open-cadastro-produto', 'open-cadastro-colecao'])
</script>

<template>
  <div>
    <div
    >
    </div>
    <div class="p-8 pt-4 pb-12">
      <h3 class="text-xl font-bold text-slate-900 mb-2">
        {{props.title}}
      </h3>
      <p class="text-sm text-slate-500 mb-8">
      </p>
      <div class="grid gap-4">
        <ActionCard
            title="Novo Produto"
            image = ""
            subtitle="Adicionar um novo produto a sua loja"
            color="indigo"
            @click="emit('open-cadastro-produto')"
        >
          <template #icon>
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-6 h-6">
              <path stroke-linecap="round" stroke-linejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
            </svg>
          </template>
        </ActionCard>

        <ActionCard
            title="Cadastrar Coleção"
            subtitle="Crie uma nova coleção"
            color="amber"
            @click="emit('open-cadastro-colecao')"
        >
          <template #icon>
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-6 h-6">
              <path stroke-linecap="round" stroke-linejoin="round" d="M16.862 4.487l1.687-1.688a1.875 1.875 0 112.652 2.652L10.582 16.07a4.5 4.5 0 01-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 011.13-1.897l8.932-8.931zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0115.75 21H5.25A2.25 2.25 0 013 18.75V8.25A2.25 2.25 0 015.25 6H10" />
            </svg>
          </template>
        </ActionCard>
      </div>

    </div>
  </div>
</template>

<style scoped>

</style>