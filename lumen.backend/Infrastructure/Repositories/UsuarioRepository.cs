namespace Infrastructure.Repositories;

public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context) {}
    
    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> ObterPorSlugAsync(string slug)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Slug == slug);
    }
}