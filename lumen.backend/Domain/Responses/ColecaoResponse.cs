namespace Domain.Responses;
public class ColecaoResponse
{
    public Guid Id { get; set; } 
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string UrlCapa { get; set; } = string.Empty;
    public bool Publica { get; set; }
    
    public Guid UsuarioId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public List<ProdutoSimplificadoResponse> Produtos { get; set; } = new();
    
    public int QuantidadeProdutos => Produtos?.Count ?? 0;
}