namespace Domain.Requests;

public class GerarLinkCheckoutRequest
{
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string TipoEntrega { get; set; }
    public string NumeroLoja { get; set; }
    public string ChavePixLoja { get; set; } 
    public string Endereco { get; set; }
    public string Pagamento { get; set; }
    public decimal Total { get; set; }
    public List<PedidoItem> Itens { get; set; } = new List<PedidoItem>();
}
