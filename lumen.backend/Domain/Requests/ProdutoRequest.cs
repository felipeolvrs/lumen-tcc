namespace Domain.Requests;

public class ProdutoRequest
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
    public decimal Preco { get; set; }

    public string UrlImagem { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string? LinkAfiliado { get; set; }

    public string? ReviewPessoal { get; set; }
    public string? Plataforma { get; set; }

    public Guid? UsuarioId { get; set; }
}