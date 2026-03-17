namespace Infrastructure.Repositories;

public class ColecaoRepository : RepositoryBase<Colecao>, IColecaoRepository
{
    public ColecaoRepository(AppDbContext context) : base(context) { }

    public override async Task<Colecao?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(c => c.Produtos)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Colecao>> ObterPorUsuarioAsync(Guid userId)
    {
        return await DbSet
            .AsNoTracking()
            .Include(c => c.Produtos)
            .Where(c => c.UsuarioId == userId)
            .ToListAsync();
    }

    public async Task AdicionarProdutoAsync(Guid colecaoId, Guid produtoId)
    {
        var colecao = await DbSet
            .Include(c => c.Produtos)
            .FirstOrDefaultAsync(c => c.Id == colecaoId);

        if (colecao is null) 
            return;
        
        colecao.Produtos ??= new List<Produto>();

        if (!colecao.Produtos.Any(p => p.Id == produtoId))
        {
            var produto = await Context.Produtos.FindAsync(produtoId);
            
            if (produto is not null)
            {
                colecao.Produtos.Add(produto);
                await Context.SaveChangesAsync();
            }
        }
    }

    public async Task RemoverProdutoAsync(Guid colecaoId, Guid produtoId)
    {
        var colecao = await DbSet
            .Include(c => c.Produtos)
            .FirstOrDefaultAsync(c => c.Id == colecaoId);

        if (colecao is null)
            return;

        var produto = colecao.Produtos.FirstOrDefault(p => p.Id == produtoId);
        
        if (produto is not null)
        {
            colecao.Produtos.Remove(produto);
            await Context.SaveChangesAsync();
        }
    }
}