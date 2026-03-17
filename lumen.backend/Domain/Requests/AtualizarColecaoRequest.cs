namespace Domain.Requests;

public class AtualizarColecaoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string UrlCapa { get; set; } = string.Empty;
    public bool Publica { get; set; } = true;
    public List<Guid> ProdutosIds { get; set; } = new();
}

