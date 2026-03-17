public class CriarPedidoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TipoEntrega { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string Pagamento { get; set; } = string.Empty;
    public string Observacao { get; set; } = string.Empty;
    public List<PedidoItem> Itens { get; set; } = new();
}