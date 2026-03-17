namespace Domain.Entities;

public class LojaConfig : EntityBase
{
    public Guid UsuarioId { get; set; }
    public virtual Usuario? Usuario { get; set; }
    public string NomeLoja { get; set; } = "Minha loja";
    public string Slug { get; set; }
    public string? Bio { get; set; }
    public string? UrlLogo { get; set; }
    public string? UrlBanner { get; set; }
    public string? WhatsApp { get; set; }
    public string? Instagram { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime UltimaAtualizacao { get; set; }
}