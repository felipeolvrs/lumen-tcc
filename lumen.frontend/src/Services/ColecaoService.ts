import { HttpService, type ApiResponse } from './ServiceBase.ts';
import { type Produto } from './ProdutoService.ts';

export interface CriarColecaoRequest {
    nome: string;
    descricao: string;
    urlCapa: string;
    produtosIds: string[];
    publica?: boolean;
}

export interface AtualizarColecaoRequest {
    nome: string;
    descricao: string;
    urlCapa: string;
    produtosIds: string[];
    publica?: boolean;
}

    export interface ColecaoResponse {
        id: string;
        nome: string;
        descricao: string;
        urlCapa: string;
        produtos: Produto[];
    }

    class ColecaoService extends HttpService {
        constructor() {
            const url = import.meta.env.VITE_API_URL;
            super(url);
        }

    async listar(): Promise<ApiResponse<ColecaoResponse[]>> {
        return await this.get<ColecaoResponse[]>('/colecoes');
    }

        async obterPorId(id: string): Promise<ApiResponse<ColecaoResponse>> {
            return await this.get<ColecaoResponse>(`/colecoes/${id}`);
        }

    async criar(dto: CriarColecaoRequest): Promise<ApiResponse<ColecaoResponse>> {
        return await this.post<ColecaoResponse>('/colecoes', dto);
    }

    async atualizar(id: string, dto: AtualizarColecaoRequest): Promise<ApiResponse<void>> {
        return await this.put<void>(`/colecoes/${id}`, dto);
    }

    async adicionarProduto(id: string, produtoId: string): Promise<ApiResponse<void>> {
        return await this.post<void>(`/colecoes/${id}/produtos`, { produtoId });
    }

    async removerProduto(id: string, produtoId: string): Promise<ApiResponse<void>> {
        return await this.delete<void>(`/colecoes/${id}/produtos/${produtoId}`);
    }

    async excluir(id: string): Promise<ApiResponse<void>> {
        return await this.delete<void>(`/colecoes/${id}`);
    }
}

export const colecaoService = new ColecaoService();