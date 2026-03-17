namespace Domain.Entities;

public class Produto : EntityBase
{
    public string Nome { get; set; }
    public string Descricao { get; set; }
    public decimal Preco { get; set; }
    public string UrlImagem { get; set; }
    public string? LinkAfiliado { get; set; }
    public string Categoria { get; set; }
    public string? ReviewPessoal { get; set; }

    public Guid? UsuarioId { get; set; }
    public virtual Usuario? Usuario { get; set; }

    public Guid? ColecaoId { get; set; }
    public virtual Colecao? Colecao { get; set; }
}
