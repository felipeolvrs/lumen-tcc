import { HttpService, type ApiResponse } from './ServiceBase.ts';

export interface WhatsappResponse {
    link: string;
}

export interface PixResponse {
    pixCopiaECola: string;
}

export interface GerarLinkCheckoutRequest {
    nome: string;
    telefone: string;
    tipoEntrega: string;
    endereco: string;
    pagamento: string;
    total: number;
    numeroLoja: string;
    chavePixLoja: string;
    itens: Array<{ nome: string; quantidade: number; preco: number }>;
}

export interface GerarLinkCheckoutResponse {
    linkWhatsapp: string;
}

class FerramentaService extends HttpService {
    constructor() {
        const url = import.meta.env.VITE_API_URL;
        super(url);
    }

    async gerarLinkCheckout(dados: GerarLinkCheckoutRequest): Promise<GerarLinkCheckoutResponse> {
        const response = await this.axiosInstance.post<GerarLinkCheckoutResponse>(
            '/ferramentas/checkout-whatsapp',
            dados
        );
        return response.data;
    }

    async gerarQrCode(url: string): Promise<Blob> {
        const response = await this.axiosInstance.get('/ferramentas/qrcode', {
            params: { url },
            responseType: 'blob'
        });
        return response.data;
    }

    async gerarLinkWhatsapp(numero: string, msg: string): Promise<ApiResponse<WhatsappResponse>> {
        const response = await this.axiosInstance.get<ApiResponse<WhatsappResponse>>('/ferramentas/whatsapp', {
            params: { numero, msg }
        });
        return response.data;
    }

    async gerarPix(chave: string, nome: string, cidade: string, valor?: number): Promise<ApiResponse<PixResponse>> {
        const response = await this.axiosInstance.get<ApiResponse<PixResponse>>('/ferramentas/pix', {
            params: { chave, nome, cidade, valor: valor || 0 }
        });
        return response.data;
    }
}

export const ferramentaService = new FerramentaService();