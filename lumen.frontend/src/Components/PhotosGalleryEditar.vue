<script setup lang="ts">
import { ref, computed } from 'vue';

const props = defineProps<{
  modelValue: string[];
}>();

const emit = defineEmits<{
  (e: 'update:modelValue', value: string[]): void;
}>();

const currentIndex = ref(0);
const fileInput = ref<HTMLInputElement | null>(null);

const displayImage = computed(() => {
  if (props.modelValue && props.modelValue.length > 0) {
    return props.modelValue[currentIndex.value] || props.modelValue[0];
  }
  return 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?ixlib=rb-1.2.1&auto=format&fit=crop&w=800&q=80';
});

const triggerFileInput = () => fileInput.value?.click();

const handleFileChange = (event: Event) => {
  const input = event.target as HTMLInputElement;
  if (input.files && input.files[0]) {
    const reader = new FileReader();
    reader.onload = (e) => {
      if (e.target?.result) {
        const newPhotos = [...props.modelValue];
        newPhotos[currentIndex.value] = e.target.result as string;
        emit('update:modelValue', newPhotos);
      }
    };
    reader.readAsDataURL(input.files[0]);
  }
  input.value = '';
};

const removePhoto = () => {
  const newPhotos = [...props.modelValue];
  newPhotos.splice(currentIndex.value, 1);
  emit('update:modelValue', newPhotos);
  if (currentIndex.value >= newPhotos.length) {
    currentIndex.value = Math.max(0, newPhotos.length - 1);
  }
};
</script>

<template>
  <div class="mb-8">
    <div class="relative w-full aspect-square bg-slate-100 rounded-3xl overflow-hidden border border-slate-100 group">
      <input ref="fileInput" type="file" accept="image/*" class="hidden" @change="handleFileChange" />

      <img :src="displayImage" class="w-full h-full object-cover transition duration-700 group-hover:scale-105" />

      <button v-if="props.modelValue.length > 0" @click.prevent="removePhoto" class="absolute top-4 right-4 bg-red-500 text-white p-2 rounded-xl shadow-lg z-10">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
        </svg>
      </button>

      <button @click.prevent="triggerFileInput" class="absolute bottom-4 right-4 bg-white text-slate-900 p-3 rounded-2xl shadow-lg z-10">
        <svg xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M3 9a2 2 0 012-2h.93a2 2 0 001.664-.89l.812-1.22A2 2 0 0110.07 4h3.86a2 2 0 011.664.89l.812 1.22A2 2 0 0018.07 7H19a2 2 0 012 2v9a2 2 0 01-2 2H5a2 2 0 01-2-2V9z" />
          <path stroke-linecap="round" stroke-linejoin="round" d="M15 13a3 3 0 11-6 0 3 3 0 016 0z" />
        </svg>
      </button>

      <div v-if="props.modelValue.length > 1" class="absolute bottom-4 left-1/2 -translate-x-1/2 flex gap-1.5 px-3 py-1.5 bg-black/20 backdrop-blur-md rounded-full">
        <button v-for="(_, i) in props.modelValue" :key="i" @click.prevent="currentIndex = i" class="w-1.5 h-1.5 rounded-full transition-colors" :class="i === currentIndex ? 'bg-white' : 'bg-white/50'"></button>
      </div>
    </div>
  </div>
</template>