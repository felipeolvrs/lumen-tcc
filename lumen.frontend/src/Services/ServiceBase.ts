import axios, {type AxiosInstance, type AxiosRequestConfig, AxiosError, type InternalAxiosRequestConfig} from 'axios';

export interface ApiResponse<T = any> {
    success: boolean;
    data: T | null;
    message: string;
    statusCode: number;
}

export abstract class HttpService {
    protected axiosInstance: AxiosInstance;

    constructor(baseUrl: string) {
        this.axiosInstance = axios.create({
            baseURL: baseUrl,
            headers: {
                'Content-Type': 'application/json'
            }
        });

        this.axiosInstance.interceptors.request.use(
            (config: InternalAxiosRequestConfig) => {
                const token = localStorage.getItem('lumen_token');
                if (token)
                    config.headers.Authorization = `Bearer ${token}`;

                const tenantId = sessionStorage.getItem('x-tenant-id');
                if (tenantId) {
                    config.headers['x-tenant-id'] = tenantId;
                }

                return config;
            },
            (error) => {
                return Promise.reject(error);
            }
        );
    }

    private getAuthHeader(): object {
        const token = localStorage.getItem('lumen_token');
        return token ? { Authorization: `Bearer ${token}` } : {};
    }

    private async request<T>(method: string, url: string, data?: any): Promise<ApiResponse<T>> {
        const config: AxiosRequestConfig = {
            method,
            url,
            data,
            headers: {
                'Content-Type': 'application/json',
                ...this.getAuthHeader()
            }
        };

        try {
            const response = await this.axiosInstance.request<T>(config);
            return {
                success: true,
                statusCode: response.status,
                message: "Sucesso",
                data: response.data
            };
        } catch (error: any) {
            const axiosError = error as AxiosError;
            const errorData = axiosError.response?.data as any;
            const mensagemErro = errorData?.error || errorData?.message || "Erro de conexão";

            return {
                success: false,
                statusCode: axiosError.response?.status || 0,
                message: mensagemErro,
                data: errorData
            };
        }
    }

    protected get<T>(url: string){
        return this.request<T>('GET', url);
    }

    protected post<T>(url: string, data: any){
        return this.request<T>('POST', url, data);
    }

    protected put<T>(url: string, data: any){
        return this.request<T>('PUT', url, data);
    }

    protected delete<T>(url: string){
        return this.request<T>('DELETE', url);
    }
}