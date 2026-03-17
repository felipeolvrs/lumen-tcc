namespace Domain.Entities;

public class Colecao : EntityBase
{
    public string Nome { get; set; }
    public string UrlCapa { get; set; }
    public string Descricao { get; set; }

    public bool Publica { get; set; } = false;
    public Guid UsuarioId { get; set; }
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}