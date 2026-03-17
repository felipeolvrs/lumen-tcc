namespace Domain.Interfaces.Repositories;

public interface IProdutoRepository : IRepositoryBase<Produto>
{
    Task<List<string>> ListarCategoriasDoUsuarioAsync(Guid id);
    Task<(List<Produto> Itens, int Total)> BuscarProdutosVitrineAsync(
        Guid usuarioId, 
        string? categoria, 
        int pagina, 
        int tamanhoPagina
    );
}