namespace Domain.Interfaces.Services;

public interface IColecaoService
{
    Task<List<ColecaoResponse>> ObterPorUsuarioAsync(Guid userId);
    Task<ColecaoResponse?> ObterPorIdAsync(Guid id);
    Task<ColecaoResponse> CriarAsync(Guid userId, CriarColecaoRequest dto);
    Task AtualizarAsync(Guid id, Guid userId, AtualizarColecaoRequest dto);
    Task ExcluirAsync(Guid id);
}