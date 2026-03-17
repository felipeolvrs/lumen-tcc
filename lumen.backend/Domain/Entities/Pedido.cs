namespace Domain.Entities;

public class Pedido : EntityBase
{
    public Guid LojaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TipoEntrega { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string Pagamento { get; set; } = string.Empty;
    public string Observacao { get; set; } = string.Empty;
    
    public string Status { get; set; } = "Pendente";
    public decimal Total { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public List<PedidoItem> Itens { get; set; } = new();
}