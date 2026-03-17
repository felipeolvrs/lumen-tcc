<script setup lang="ts">
interface VerticalCardProps {
  title: string
  category: string
  image: string
}

withDefaults(defineProps<VerticalCardProps>(), {
  title: "titulo",
  category:"categoria"
})

const emit = defineEmits(['click'])
</script>

<template>
  <article
      @click="emit('click')"
      class="snap-start relative flex-shrink-0 w-[150px] h-[200px] rounded-2xl overflow-hidden group cursor-pointer shadow-md active:scale-95 transition-transform duration-200"
  >
    <img
        :src="image"
        :alt="title"
        class="w-full h-full object-cover transition duration-700 group-hover:scale-110"
    />
    <div class="absolute inset-0 bg-gradient-to-t from-slate-900/90 via-slate-900/20 to-transparent"></div>

    <div class="absolute bottom-0 left-0 right-0 p-3.5">
      <div class="flex items-center gap-1 mb-1">
        <span class="w-1.5 h-1.5 rounded-full bg-white"></span>
        <p class="text-[9px] text-slate-200 font-semibold uppercase tracking-widest">
          {{ category }}
        </p>
      </div>
      <h3 class="text-white text-sm font-bold leading-tight">
        {{title}}  </h3>
    </div>
  </article>
</template>

<style scoped>

</style>