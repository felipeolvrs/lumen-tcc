namespace Domain.Responses;

public class PedidoResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
}