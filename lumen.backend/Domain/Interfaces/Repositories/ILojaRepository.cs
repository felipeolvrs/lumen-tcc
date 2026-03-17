namespace Domain.Interfaces.Repositories;

public interface ILojaRepository
{
    Task<LojaConfig?> GetByUserIdAsync(Guid userId);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> AnyWithSlugAsync(string slug, Guid userId);
    Task AddAsync(LojaConfig loja);
    Task UpdateAsync(LojaConfig loja);
}
