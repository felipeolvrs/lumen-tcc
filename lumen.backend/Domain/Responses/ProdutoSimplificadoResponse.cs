namespace Domain.Responses;

public class ProdutoSimplificadoResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string UrlImagem { get; set; } = string.Empty;
}