namespace Domain.Interfaces.Services;

public interface IPedidoService
{
    Task<PedidoResponse> CriarPedidoAsync(CriarPedidoRequest request);
}