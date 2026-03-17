namespace Domain.Interfaces.Repositories;

public interface IUsuarioRepository : IRepositoryBase<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario?> ObterPorSlugAsync(string slug);
}