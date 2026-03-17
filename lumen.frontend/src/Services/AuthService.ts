import { HttpService, type ApiResponse } from './ServiceBase';

export interface LoginCredentials {
    email: string;
    senha: string;
}

export interface RegisterDTO {
    nome: string;
    email: string;
    senha: string;}

export interface AuthResponse {
    id: string;
    token: string;
    nome: string;
    email: string;
}

class AuthService extends HttpService {
    constructor() {
        super(import.meta.env.VITE_API_URL);
    }

    async login(credentials: LoginCredentials): Promise<ApiResponse<AuthResponse>> {
        const response = await this.post<AuthResponse>('/auth/login', credentials);

        if (response.success && response.data?.token) {
            localStorage.setItem('lumen_token', response.data.token);
            localStorage.setItem('lumen_user', JSON.stringify(response.data));
        }

        return response;
    }

    async cadastrar(dados: RegisterDTO): Promise<ApiResponse<void>> {
        return await this.post<void>('/auth/registrar', dados);
    }

    logout(): void {
        localStorage.removeItem('lumen_token');
        localStorage.removeItem('lumen_user');
        window.location.href = '/login';
    }

    isAuthenticated(): boolean {
        return !!localStorage.getItem('lumen_token');
    }
}

export const authService = new AuthService();