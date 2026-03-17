namespace Domain.Requests;

public class AtualizarUsuarioRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Senha { get; set; }
    public string? Username { get; set; } 
    public string? FotoUrl { get; set; }  
    public string? Whatsapp { get; set; }
    public string? TipoLoja { get; set; } 
    public string? Bio { get; set; }
    public string? NomeLoja { get; set; }
    public string? Instagram { get; set; }
    public string? ChavePix { get; set; }

    public string? UrlBanner { get; set; }
    public bool Ativo { get; set; } 
}