namespace Domain.Interfaces.Repositories;

public interface IColecaoRepository : IRepositoryBase<Colecao>
{
    Task<IEnumerable<Colecao>> ObterPorUsuarioAsync(Guid userId);

    Task AdicionarProdutoAsync(Guid colecaoId, Guid produtoId);

    Task RemoverProdutoAsync(Guid colecaoId, Guid produtoId);
}