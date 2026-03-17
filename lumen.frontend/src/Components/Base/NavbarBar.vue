<script setup lang="ts">
import { useRoute } from "vue-router";
import { useCartStore } from "../../Store/useCartStore.ts";
import NavItem from "./NavItem.vue";
import AddButton from "./AddButton.vue";

const route = useRoute();
const cart = useCartStore();

const DEFAULT_AVATAR = "https://cdn-icons-png.flaticon.com/512/149/149071.png";

defineProps<{
  userAvatar?: string;
}>();

defineEmits([
  'open-cart',
  'open-add',
  'open-perfil',
  'open-kits',
  'close-all',
  'go-home'
]);
</script>

<template>
  <nav class="fixed bottom-0 left-0 right-0 bg-white/90 backdrop-blur-xl border-t border-slate-100 px-6 py-2 flex justify-between items-center z-30 max-w-md mx-auto pb-safe shadow-[0_-4px_12px_rgba(0,0,0,0.03)]">

    <NavItem
        text="Início"
        :is-active="route.path === '/'"
        @click="$emit('go-home')"
    >
      <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="currentColor" class="w-6 h-6" :class="route.path === '/' ? 'text-slate-900' : 'text-slate-400'">
        <path d="M11.47 3.84a.75.75 0 011.06 0l8.69 8.69a.75.75 0 101.06-1.06l-8.689-8.69a2.25 2.25 0 00-3.182 0l-8.69 8.69a.75.75 0 001.061 1.06l8.69-8.69z" />
        <path d="M12 5.432l8.159 8.159c.03.03.06.058.091.086v6.198c0 1.035-.84 1.875-1.875 1.875H15a.75.75 0 01-.75-.75v-4.5a.75.75 0 00-.75-.75h-3a.75.75 0 00-.75.75V21a.75.75 0 01-.75.75H5.625a1.875 1.875 0 01-1.875-1.875v-6.198a2.29.29 0 00.091-.086L12 5.43z" />
      </svg>
    </NavItem>

    <NavItem
        text="Kits"
        :is-active="route.path.includes('/colecao')"
        @click="$emit('open-kits')"
    >
      <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-6 h-6" :class="route.path.includes('/colecao') ? 'text-slate-900' : 'text-slate-400'">
        <path stroke-linecap="round" stroke-linejoin="round" d="M3.75 12h16.5m-16.5 3.75h16.5M3.75 19.5h16.5M5.625 4.5h12.75a1.875 1.875 0 011.875 1.875v10.5a1.875 1.875 0 01-1.875 1.875H5.625a1.875 1.875 0 01-1.875-1.875V6.375a1.875 1.875 0 011.875-1.875z" />
      </svg>
    </NavItem>

    <AddButton :is-admin="true" @click="$emit('open-add')" />

    <NavItem text="Carrinho" @click="$emit('open-cart')">
      <div class="relative">
        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="w-6 h-6 text-slate-400">
          <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 10.5V6a3.75 3.75 0 10-7.5 0v4.5m11.356-1.993l1.263 12c.07.665-.45 1.243-1.119 1.243H4.25a1.125 1.125 0 01-1.12-1.243l1.264-12A1.125 1.125 0 015.513 7.5h12.974c.576 0 1.059.435 1.119 1.007zM8.625 10.5a.375.375 0 11-.75 0 .375.375 0 01.75 0zm7.5 0a.375.375 0 11-.75 0 .375.375 0 01.75 0z" />
        </svg>

        <Transition name="pop">
          <div
              v-if="cart.count.value > 0"
              class="absolute -top-2 -right-2 bg-blue-800 text-white text-[9px] font-black w-5 h-5 rounded-full flex items-center justify-center border-2 border-white shadow-sm"
          >
            {{ cart.count.value }}
          </div>
        </Transition>
      </div>
    </NavItem>

    <NavItem
        text="Perfil"
        :is-active="route.path === '/perfil'"
        @click="$emit('open-perfil')"
    >
      <div class="w-7 h-7 rounded-full overflow-hidden border-2 transition-all"
           :class="route.path === '/perfil' ? 'border-slate-900 scale-110' : 'border-slate-100'">
        <img
            :src="userAvatar || DEFAULT_AVATAR"
            class="w-full h-full object-cover"
            alt="Avatar"
        />
      </div>
    </NavItem>
  </nav>
</template>

<style scoped>
.pop-enter-active {
  animation: pop-in 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275);
}
.pop-leave-active {
  animation: pop-in 0.2s reverse ease-in;
}

@keyframes pop-in {
  0% { transform: scale(0); }
  100% { transform: scale(1); }
}
</style>