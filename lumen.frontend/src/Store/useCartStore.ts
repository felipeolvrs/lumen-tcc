import { reactive, computed } from 'vue';
import type { Produto } from '../Services/ProdutoService';

export interface CartItem {
    produto: Produto;
    quantidade: number;
}

const state = reactive<{ items: CartItem[] }>({
    items: [],
});

export function useCartStore() {
    const items = computed(() => state.items);

    const total = computed(() =>
        state.items.reduce((acc, item) => acc + item.produto.preco * item.quantidade, 0)
    );

    const count = computed(() =>
        state.items.reduce((acc, item) => acc + item.quantidade, 0)
    );

    function addItem(produto: Produto) {
        const existing = state.items.find((i) => i.produto.id === produto.id);
        if (existing) {
            existing.quantidade++;
        } else {
            state.items.push({ produto, quantidade: 1 });
        }
    }

    function removeItem(produtoId: string) {
        const index = state.items.findIndex((i) => i.produto.id === produtoId);
        if (index > -1) {
            state.items.splice(index, 1);
        }
    }

    function updateQuantity(produtoId: string, quantidade: number) {
        const item = state.items.find((i) => i.produto.id === produtoId);
        if (item) {
            if (quantidade <= 0) {
                removeItem(produtoId);
            } else {
                item.quantidade = quantidade;
            }
        }
    }

    function clearCart() {
        state.items.splice(0, state.items.length);
    }

    return { items, total, count, addItem, removeItem, updateQuantity, clearCart };
}