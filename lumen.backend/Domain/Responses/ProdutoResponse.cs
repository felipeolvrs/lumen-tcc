namespace Domain.Responses;

public class ProdutoResponse
{
    public Guid Id { get; set; }
    public Guid? UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string UrlImagem { get; set; } = string.Empty;
    public string? LinkAfiliado { get; set; } = string.Empty;
    public string? ReviewPessoal { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool Ativo { get; set; }
}