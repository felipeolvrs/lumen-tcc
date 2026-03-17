namespace Domain.Entities;

public class Usuario : EntityBase
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public Perfil Perfil { get; set; }
    public string? Slug { get; set; } 
    public string? NomeLoja { get; set; } 
    public string? TipoLoja { get; set; }

    public string? Bio { get; set; }
    public string? UrlLogo { get; set; }
    public string? UrlBanner { get; set; }
    public string? WhatsApp { get; set; }
    public string? Instagram { get; set; }
    public string? ChavePix { get; set; }
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}