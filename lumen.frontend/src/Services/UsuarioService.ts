import { HttpService, type ApiResponse } from './ServiceBase.ts';

export interface UsuarioPerfil {
    id?: string;
    nome: string;
    email?: string;
    slug?: string;
    fotoUrl?: string;
    urlLogo?: string;
    urlBanner?: string;
    whatsApp?: string;
    tipoLoja?: 'products' | 'services' | 'affiliate' | 'hybrid';
    bio?: string;
    nomeLoja?: string;
    instagram?: string;
    chavePix?: string;
    ativo: boolean;
    senha?: string;
}

class UsuarioService extends HttpService {
    constructor() {
        const url = import.meta.env.VITE_API_URL;
        super(url);
    }

    async obterPerfil(): Promise<ApiResponse<UsuarioPerfil>> {
        return await this.get<UsuarioPerfil>('/usuarios/me');
    }

    async atualizarPerfil(dados: UsuarioPerfil): Promise<ApiResponse<void>> {
        return await this.put<void>('/usuarios/me', dados);
    }

    async obterPorSlug(slug: string): Promise<ApiResponse<UsuarioPerfil>> {
        return await this.get<UsuarioPerfil>(`/usuarios/vitrine/${slug}`);
    }
}

export const usuarioService = new UsuarioService();