namespace Domain.Responses;

public class CriarColecaoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    
    public string UrlCapa { get; set; } = string.Empty;
    public List<Guid> ProdutosIds { get; set; } = new(); 
    
    public bool Publica { get; set; } = true;
}