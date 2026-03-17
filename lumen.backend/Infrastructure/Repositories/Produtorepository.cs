namespace Infrastructure.Repositories;

public class ProdutoRepository : RepositoryBase<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context) { }
    
    public async Task<List<string>> ListarCategoriasDoUsuarioAsync(Guid userId)
    {
        return await DbSet
            .AsNoTracking()
            .Where(p => p.UsuarioId == userId)
            .Select(p => p.Categoria)
            .Distinct()
            .ToListAsync();
    }

    public IQueryable<Produto> GetQueryable()
    {
        return DbSet.AsNoTracking().AsQueryable();
    }
    
    public async Task<(List<Produto> Itens, int Total)> BuscarProdutosVitrineAsync(
        Guid usuarioId, 
        string? categoria, 
        int pagina, 
        int tamanhoPagina)
    {
        var query = DbSet.AsNoTracking()
            .Where(p => p.UsuarioId == usuarioId && p.Ativo);

        if (!string.IsNullOrWhiteSpace(categoria))
            query = query.Where(p => p.Categoria.ToLower() == categoria.ToLower());
        
        var total = await query.CountAsync();
        
        var itens = await query
            .OrderByDescending(p => p.CreatedAt) 
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        return (itens, total);
    }
}