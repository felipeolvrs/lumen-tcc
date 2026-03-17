<script setup lang="ts">
interface DrawerProps {
  isOpen: boolean
}

defineProps<DrawerProps>()
defineEmits(['close'])
</script>

<template>
  <div v-if="isOpen" class="fixed inset-0 z-[60]">
    <Transition
        appear
        enter-active-class="transition-opacity duration-300 ease-out"
        enter-from-class="opacity-0"
        enter-to-class="opacity-100"
        leave-active-class="transition-opacity duration-200 ease-in"
        leave-from-class="opacity-100"
        leave-to-class="opacity-0"
    >
      <div
          class="absolute inset-0 bg-slate-900/60 backdrop-blur-md"
          @click="$emit('close')"
      />
    </Transition>

    <Transition
        appear
        enter-active-class="transition-transform duration-400 cubic-bezier(0.32, 0.72, 0, 1)"
        enter-from-class="translate-y-full"
        enter-to-class="translate-y-0"
        leave-active-class="transition-transform duration-300 ease-in"
        leave-from-class="translate-y-0"
        leave-to-class="translate-y-full"
    >
      <div class="absolute inset-x-0 bottom-0 max-h-[92vh] w-full max-w-md mx-auto bg-white rounded-t-[40px] shadow-2xl flex flex-col overflow-hidden">

        <div class="w-full flex justify-center py-4" @click="$emit('close')">
          <div class="w-12 h-1.5 bg-slate-100 rounded-full"></div>
        </div>

        <div class="flex-1 overflow-y-auto overflow-x-hidden pb-safe">
          <slot />
        </div>
      </div>
    </Transition>
  </div>
</template>

<style scoped>
.pb-safe {
  padding-bottom: env(safe-area-inset-bottom);
}
</style>