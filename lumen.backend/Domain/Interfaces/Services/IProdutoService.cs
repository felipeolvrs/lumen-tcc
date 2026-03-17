namespace Domain.Interfaces.Services;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoResponse>> ObterTodosAsync();
    Task<ProdutoResponse?> ObterPorIdAsync(Guid id);
    Task<List<string>> ListarCategoriasDoUsuarioAsync(Guid userId);
    Task<ProdutoResponse> CriarAsync(Guid userId, ProdutoRequest dto);
    Task<bool> AtualizarAsync(Guid id, Guid userId, ProdutoRequest dto);
    Task<bool> ExcluirAsync(Guid id, Guid userId);
}