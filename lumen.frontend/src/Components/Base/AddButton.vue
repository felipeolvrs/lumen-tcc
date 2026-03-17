<script setup lang="ts">
interface AddButtonProps {
  isAdmin?: boolean
}

withDefaults(defineProps<AddButtonProps>(), {
  isAdmin: false
})

defineEmits(['click'])
</script>

<template>
  <div v-if="isAdmin" class="relative -top-6">
    <button
        type="button"
        @click="$emit('click')"
        class="w-14 h-14 bg-slate-900 rounded-full flex items-center justify-center text-white shadow-xl shadow-slate-900/30 active:scale-90 transition-transform duration-200 ring-4 ring-white"
    >
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-8 h-8">
        <path stroke-linecap="round" stroke-linejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
      </svg>
    </button>
  </div>
</template>