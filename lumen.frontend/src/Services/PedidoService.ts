import { HttpService, type ApiResponse } from './ServiceBase.ts';

export interface PedidoItem {
    id: string | number;
    nome: string;
    quantidade: number;
    preco: number;
}

export interface Pedido {
    nome: string;
    telefone: string;
    email: string;
    tipoEntrega: string;
    endereco: string;
    pagamento: string;
    observacao: string;
    total: number;
    itens: PedidoItem[];
}

class PedidoService extends HttpService {
    constructor() {
        const url = import.meta.env.VITE_API_URL;
        super(url);
    }

    async finalizar(pedido: Pedido): Promise<ApiResponse<Pedido>> {
        return await this.post<Pedido>('/pedidos/criar', pedido);
    }
}

export const pedidoService = new PedidoService();