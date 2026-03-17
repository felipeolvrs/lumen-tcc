<script setup lang="ts">
import { ref, computed } from "vue";

interface PhotosGalleryProps {
  images: string[]
}

const props = withDefaults(defineProps<PhotosGalleryProps>(), {
  images: () => []
})

const currentIndex = ref(0)

const DEFAULT_IMAGE = 'https://placehold.co/600x600/f1f5f9/94a3b8?text=Sem+Foto';

const validImages = computed(() => {
  const filtered = props.images.filter(img => img && img.trim() !== '');
  return filtered.length > 0 ? filtered : [DEFAULT_IMAGE];
})

const setIndex = (index: number) => {
  currentIndex.value = index
}
</script>

<template>
  <div class="w-full aspect-square bg-slate-50 relative group overflow-hidden rounded-b-[40px] shadow-inner">
    <Transition name="fade" mode="out-in">
      <img
          :key="validImages[currentIndex]"
          :src="validImages[currentIndex]"
          class="w-full h-full object-cover"
          alt="Imagem do Produto"
      />
    </Transition>

    <div
        v-if="validImages.length > 1"
        class="absolute bottom-6 left-1/2 -translate-x-1/2 flex gap-2 bg-slate-900/10 backdrop-blur-md px-3 py-2 rounded-full"
    >
      <button
          v-for="(_, index) in validImages"
          :key="index"
          @click="setIndex(index)"
          class="h-1.5 rounded-full transition-all duration-300"
          :class="index === currentIndex ? 'w-6 bg-slate-900' : 'w-1.5 bg-slate-900/20'"
          :aria-label="`Ir para imagem ${index + 1}`"
      ></button>
    </div>
  </div>
</template>

<style scoped>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>