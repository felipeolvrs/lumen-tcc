namespace Domain.Interfaces.Services;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> ObterTodosAsync();
    Task<Usuario?> ObterPorIdAsync(Guid id);
    Task<UsuarioResponse?> ObterPerfilPorIdAsync(Guid id);
    Task<UsuarioResponse?> ObterPorSlugAsync(string slug);
    Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest dto);
    Task<bool> AtualizarAsync(Guid id, AtualizarUsuarioRequest dto);
    Task<bool> ExcluirAsync(Guid id);
}