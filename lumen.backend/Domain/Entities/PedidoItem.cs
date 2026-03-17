namespace Domain.Entities;

public class PedidoItem : EntityBase
{
    public Guid PedidoId { get; set; }
    public Guid ProdutoId { get; set; } 
    public string Nome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal Preco { get; set; }
}