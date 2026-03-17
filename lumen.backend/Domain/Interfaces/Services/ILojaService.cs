namespace Domain.Interfaces.Services;

public interface ILojaService
{
    Task<LojaResponse?> ObterConfiguracaoAsync(Guid userId);
    Task<bool> SlugJaExisteAsync(string slug, Guid userId);
    Task AtualizarConfiguracaoAsync(Guid userId, LojaRequest dto);
}