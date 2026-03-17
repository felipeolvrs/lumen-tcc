<script setup lang="ts">
interface SheetProps {
  isOpen: boolean
}

defineProps<SheetProps>()
defineEmits(['close'])
</script>

<template>
    <div
        v-if="isOpen"
        class="absolute inset-0 z-[500] flex flex-col justify-end overflow-hidden"
    >

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
            class="absolute inset-0 bg-slate-900/40 backdrop-blur-sm"
            @click="$emit('close')"
        />
      </Transition>

      <Transition
          appear
          enter-active-class="transition-transform duration-300 cubic-bezier(0.16, 1, 0.3, 1)"
          enter-from-class="translate-y-full"
          enter-to-class="translate-y-0"
          leave-active-class="transition-transform duration-200 ease-in"
          leave-from-class="translate-y-0"
          leave-to-class="translate-y-full"
      >
        <div class="relative w-full bg-white rounded-t-[32px] shadow-[0_-10px_40px_rgba(0,0,0,0.1)] flex flex-col max-h-[85vh]">

          <div
              class="w-full flex justify-center pt-4 pb-2 flex-shrink-0 cursor-pointer"
              @click="$emit('close')"
          >
            <div class="w-12 h-1.5 bg-slate-200 rounded-full"></div>
          </div>

          <div class="flex-1 overflow-y-auto overflow-x-hidden relative pb-6">
            <slot />
          </div>
        </div>
      </Transition>
    </div>
</template>