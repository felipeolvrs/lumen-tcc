namespace Domain.Interfaces.Services;

public interface IFerramentasService
{
    byte[] GerarQrCode(string url);
    string GerarLinkWhatsapp(string numero, string mensagem);
    string GerarPixEstatico(string chave, string nomeRecebedor, string cidade, decimal? valor, string txtId);
    GerarLinkCheckoutResponse GerarLinkCheckoutWhatsapp(GerarLinkCheckoutRequest request);
}