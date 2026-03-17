namespace Domain.Responses;
public class UsuarioResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Perfil Perfil { get; set; }
    public bool Ativo { get; set; }
    public string? Slug { get; set; } 
    public string? NomeLoja { get; set; } 
    public string? Bio { get; set; }
    public string? UrlLogo { get; set; }
    public string? UrlBanner { get; set; }
    public string? WhatsApp { get; set; }
    public string? Instagram { get; set; }
    public string? ChavePix { get; set; }
    public string? TipoLoja { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime? UpdatedAt { get; set; }
}