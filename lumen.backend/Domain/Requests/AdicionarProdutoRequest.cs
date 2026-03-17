namespace Domain.Requests;

public class AdicionarProdutoRequest
{
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public Guid UsuarioId { get; set; }
    
}