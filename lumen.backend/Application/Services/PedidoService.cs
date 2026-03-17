namespace Application.Services;

using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Requests;
using Domain.Responses;
using Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly ITenantService _tenantService;

    public PedidoService(IPedidoRepository pedidoRepository, ITenantService tenantService)
    {
        _pedidoRepository = pedidoRepository;
        _tenantService = tenantService;
    }

    public async Task<PedidoResponse> CriarPedidoAsync(CriarPedidoRequest request)
    {
        Guid lojaId = _tenantService.GetCurrentTenantId();

        if (lojaId == Guid.Empty)
            throw new ArgumentException("Identificação da loja (Tenant) não encontrada.");

        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new ArgumentException("O nome é obrigatório.");

        if (request.Itens is null || !request.Itens.Any())
            throw new ArgumentException("O pedido deve conter ao menos um item.");
        
        var novoPedido = new Pedido
        {
            LojaId = lojaId,
            Nome = request.Nome,
            Telefone = request.Telefone,
            Email = request.Email,
            TipoEntrega = request.TipoEntrega,
            Endereco = request.Endereco,
            Pagamento = request.Pagamento,
            Observacao = request.Observacao,
            DataCriacao = DateTime.UtcNow,
            Status = "Pendente",
            Total = request.Itens.Sum(i => i.Preco * i.Quantidade),
            Itens = request.Itens.Select(i => new PedidoItem() 
            {
                ProdutoId = i.Id,
                Nome = i.Nome,
                Quantidade = i.Quantidade,
                Preco = i.Preco,
            }).ToList()
        };

        await _pedidoRepository.AddAsync(novoPedido);

        return new PedidoResponse
        {
            Id = novoPedido.Id,
            Status = novoPedido.Status,
            Mensagem = "Pedido realizado com sucesso! Entraremos em contato."
        };
    }
}