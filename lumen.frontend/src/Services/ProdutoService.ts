import { HttpService, type ApiResponse } from './ServiceBase.ts';

export interface Produto {
    id?: string;
    nome: string;
    descricao: string;
    preco: number;
    precoAntigo: number;
    urlImagem: string;
    categoria: string;
    ativo: boolean;
    linkAfiliado?: string;
    plataforma?: string;
    reviewPessoal?: string;
}

class ProdutoService extends HttpService {
    constructor() {
        super(import.meta.env.VITE_API_URL);
    }

    async listar(): Promise<ApiResponse<Produto[]>> {
        return await this.get<Produto[]>('/produtos');
    }

    async obterPorId(id: string): Promise<ApiResponse<Produto>> {
        return await this.get<Produto>(`/produtos/${id}`);
    }

    async criar(produto: Produto): Promise<ApiResponse<Produto>> {
        return await this.post<Produto>('/produtos', produto);
    }

    async atualizar(id: string, produto: Produto): Promise<ApiResponse<void>> {
        return await this.put<void>(`/produtos/${id}`, produto);
    }

    async remover(id: string): Promise<ApiResponse<void>> {
        return await this.delete<void>(`/produtos/${id}`);
    }

    async ativar(id: string) : Promise<ApiResponse<void>> {
        return await  this.post<void>(`/produtos/${id}/ativar`,{})
    }

    async inativar(id: string) : Promise<ApiResponse<void>> {
        return await  this.post<void>(`/produtos/${id}/desativar`, {})
    }

}

export const produtoService = new ProdutoService();