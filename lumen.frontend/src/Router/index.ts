import { createRouter, createWebHistory } from 'vue-router'
import { authService } from '../Services/AuthService.ts'

import TelaLogin from "../Pages/TelaLogin.vue";
import Register from "../Pages/Register.vue";
import Home from "../Pages/Home.vue";
import Dev from "../Pages/Dev.vue";
import VisualizarProduto from "../Pages/VisualizarProduto.vue";
import CheckoutPage from "../Pages/CheckoutPage.vue";
import ColecaoProduto from "../Pages/ColecaoProduto.vue";
import Perfil from "../Pages/Perfil.vue";
import CadastroProduto from "../Pages/CadastroProduto.vue";
import CadastrarKits from "../Pages/CadastrarKits.vue";
import GerenciarProdutos from "../Pages/GerenciarProdutos.vue";

const router = createRouter({
    history: createWebHistory(),
    routes: [
        // --- ROTAS PÚBLICAS (Acesso de qualquer pessoa) ---
        { path: '/login', name: 'login', component: TelaLogin },
        { path: '/register', name: 'register', component: Register },

        // Rota da Vitrine Pública (Onde o cliente vê os produtos)
        // Colocamos o slug como parâmetro principal
        {
            path: '/',
            name: 'vitrine',
            component: Home, // Assumindo que Home exibe a lista de produtos da vitrine
            meta: { requiresAuth: false }
        },

        // Rota de Checkout Pública (O cliente final compra aqui)
        {
            path: '/:slug/checkout',
            name: 'checkout',
            component: CheckoutPage,
            meta: { requiresAuth: false }
        },

        // --- ROTAS ADMINISTRATIVAS (Exigem Login do Lojista) ---
        {
            path: '/',
            name: 'dashboard',
            component: Home,
            meta: { requiresAuth: true }
        },
        {
            path: '/perfil',
            name: 'perfil',
            component: Perfil,
            meta: { requiresAuth: true }
        },
        {
            path: '/produto/cadastro/',
            name: "produto.cadastro",
            component: CadastroProduto,
            meta: { requiresAuth: true }
        },
        {
            path: '/produto/editar/:id',
            name: "produto.editar",
            component: CadastroProduto,
            meta: { requiresAuth: true }
        },
        {
            path: '/produto/detalhes/:id',
            name: "produto.detalhes",
            component: VisualizarProduto, // Caso o lojista veja detalhes internos
            meta: { requiresAuth: true }
        },
        {
          path: '/gerenciar/produtos/',
          name: "gerenciar.produtos",
          component: GerenciarProdutos,
          meta: { requiresAuth: true }
        },
        {
            path: '/colecao/',
            name: "colecao",
            component: ColecaoProduto,
            meta: { requiresAuth: true }
        },
        {
            path: '/colecao/:id',
            name: "colecao.detalhes",
            component: ColecaoProduto,
            meta: { requiresAuth: true }
        },
        {
            path: '/colecao/cadastro/',
            name: "colecao.cadastro",
            component: CadastrarKits,
            meta: { requiresAuth: true }
        },
        {
            path: '/colecao/cadastro/editar/:id',
            name: "colecao.editar",
            component: CadastrarKits,
            meta: { requiresAuth: true }
        },
        {
            path: '/dev',
            name: 'dev',
            component: Dev,
            meta: { requiresAuth: true }
        }
    ]
})

router.beforeEach((to) => {
    const requiresAuth = to.meta.requiresAuth;
    const isAuthenticated = authService.isAuthenticated();

    if (requiresAuth && !isAuthenticated)
        return '/login';

    if ((to.name === 'login' || to.name === 'register') && isAuthenticated)
        return '/';
})

export default router