<script setup lang="ts">
import { computed } from 'vue';

interface MarketplaceOption {
  id: string;
  label: string;
  colorTheme: string;
  iconPath?: string;
  isTextIcon?: boolean;
}

interface AffiliateSectionProps {
  linkValue: string;
  selectedPlatformId: string;
  labelLink?: string;
  labelPlatform?: string;
}

const props = withDefaults(defineProps<AffiliateSectionProps>(), {
  linkValue: '',
  selectedPlatformId: 'shopee',
  labelLink: 'Link do Produto',
  labelPlatform: 'Plataforma',
});

const emit = defineEmits<{
  (e: 'update:linkValue', value: string): void;
  (e: 'update:selectedPlatformId', value: string): void;
}>();

const currentLink = computed({
  get: () => props.linkValue,
  set: (val) => emit('update:linkValue', val),
});

const currentPlatform = computed({
  get: () => props.selectedPlatformId,
  set: (val) => emit('update:selectedPlatformId', val),
});

const marketplaces: MarketplaceOption[] = [
  {
    id: 'shopee',
    label: 'Shopee',
    colorTheme: 'orange',
    iconPath: 'M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-1 15h-2v-2h2v2zm0-4h-2V7h2v6z'
  },
  {
    id: 'amazon',
    label: 'Amazon',
    colorTheme: 'yellow',
    iconPath: 'M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-1 15h-2v-2h2v2zm0-4h-2V7h2v6z' // (Usei o mesmo icone do exemplo, substitua pelo real se tiver)
  },
  {
    id: 'magalu',
    label: 'Magalu',
    colorTheme: 'blue',
    isTextIcon: true
  },
  {
    id: 'other',
    label: 'Outro',
    colorTheme: 'slate',
  }
];

const getBorderClass = (theme: string, isSelected: boolean) => {
  if (!isSelected)
    return 'border-slate-100 bg-white';

  const maps: Record<string, string> = {
    orange: 'border-orange-500 bg-orange-50',
    yellow: 'border-yellow-500 bg-yellow-50',
    blue: 'border-blue-500 bg-blue-50',
    slate: 'border-slate-800 bg-slate-50',
  };
  return maps[theme] || maps.slate;
};

const getTextClass = (theme: string) => {
  const maps: Record<string, string> = {
    orange: 'text-orange-500',
    yellow: 'text-yellow-500',
    blue: 'text-blue-500',
    slate: 'text-slate-400',
  };
  return maps[theme] || maps.slate;
};
</script>

<template>
  <div id="section-affiliate" class="fade-enter space-y-6">

    <div>
      <label class="block text-xs font-bold text-slate-500 uppercase mb-2 ml-1">
        {{ props.labelLink }}
      </label>
      <div class="relative flex items-center">
        <input
            v-model="currentLink"
            type="url"
            class="custom-input w-full bg-slate-50 border border-slate-200 rounded-2xl py-3.5 pl-4 pr-12 text-sm font-medium text-slate-900 outline-none focus:border-indigo-500 transition-colors"
            placeholder="Cole o link aqui..."
        />
        <button
            type="button"
            class="absolute right-3 p-1.5 bg-white rounded-lg text-slate-400 hover:text-indigo-600 shadow-sm border border-slate-100 transition"
        >
          <svg xmlns="http://www.w3.org/2000/svg" class="w-4 h-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" />
          </svg>
        </button>
      </div>
    </div>

    <div>
      <label class="block text-xs font-bold text-slate-500 uppercase mb-2 ml-1">
        {{ props.labelPlatform }}
      </label>

      <div class="flex gap-3 overflow-x-auto pb-4 no-scrollbar -mx-6 px-6">

        <label
            v-for="item in marketplaces"
            :key="item.id"
            class="cursor-pointer flex-shrink-0"
        >
          <input
              type="radio"
              name="marketplace"
              :value="item.id"
              v-model="currentPlatform"
              class="peer sr-only"
          />

          <div
              class="w-24 h-16 rounded-xl border-2 flex flex-col items-center justify-center gap-1 transition-all"
              :class="getBorderClass(item.colorTheme, currentPlatform === item.id)"
          >
            <svg
                v-if="item.iconPath"
                class="w-5 h-5"
                :class="getTextClass(item.colorTheme)"
                fill="currentColor"
                viewBox="0 0 24 24"
            >
              <path :d="item.iconPath" />
            </svg>

            <div
                v-else-if="item.isTextIcon"
                class="w-5 h-5 rounded-full bg-blue-500 flex items-center justify-center text-white text-[8px] font-black tracking-tighter"
            >
              Lu
            </div>

            <span
                class="text-[10px] font-bold"
                :class="currentPlatform === item.id ? 'text-slate-700' : 'text-slate-400'"
            >
              {{ item.label }}
            </span>
          </div>
        </label>

      </div>
    </div>
  </div>
</template>

<style scoped>
.no-scrollbar::-webkit-scrollbar {
  display: none;
}
.no-scrollbar {
  -ms-overflow-style: none;
  scrollbar-width: none;
}
</style>