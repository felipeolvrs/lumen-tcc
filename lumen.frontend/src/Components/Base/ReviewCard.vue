<script setup lang="ts">
import { computed } from 'vue';

interface ReviewCardProps {
  sectionTitle: string
  userName: string
  description: string
  createdAt?: Date | string
  userImage?: string
}

const props = withDefaults(defineProps<ReviewCardProps>(), {
  userImage: '',
  createdAt: ''
})

const DEFAULT_AVATAR = 'https://cdn-icons-png.flaticon.com/512/149/149071.png';

const displayImage = computed(() => {
  return props.userImage && props.userImage.length > 10 ? props.userImage : DEFAULT_AVATAR;
});
</script>

<template>
  <section class="space-y-4">
    <h3 class="text-[10px] font-black text-slate-400 uppercase tracking-[0.2em] flex items-center gap-2 ml-1">
      <span class="w-1.5 h-1.5 bg-slate-900 rounded-full"></span>
      {{ sectionTitle }}
    </h3>

    <div class="bg-slate-50 rounded-[32px] p-6 border border-slate-100 relative overflow-hidden group">

      <i class="ph-fill ph-quotes absolute -right-2 -bottom-2 text-7xl text-slate-200/50 -rotate-12 pointer-events-none"></i>

      <div class="flex items-center gap-4 mb-4 relative z-10">
        <div class="w-12 h-12 rounded-full bg-white p-1 shadow-sm shrink-0 border border-slate-100">
          <img
              :src="displayImage"
              class="w-full h-full object-cover rounded-full"
              alt="Avatar"
          />
        </div>

        <div class="min-w-0">
          <p class="text-sm font-black text-slate-900 truncate">{{ userName }}</p>
          <p v-if="createdAt" class="text-[10px] font-bold text-slate-400 mt-0.5 uppercase tracking-wider">
            {{ createdAt }}
          </p>
        </div>
      </div>

      <p class="text-slate-600 text-sm leading-relaxed relative z-10 font-medium italic">
        "{{ description }}"
      </p>
    </div>
  </section>
</template>