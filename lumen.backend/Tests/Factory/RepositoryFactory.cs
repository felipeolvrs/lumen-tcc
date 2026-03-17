using Domain.Interfaces.Repositories;
using Infrastructure;
using Infrastructure.Repositories;

namespace Tests.Factory;

public static class RepositoryFactory
{
    public static IProdutoRepository ProdutoRepo(AppDbContext dbContext) =>
        new ProdutoRepository(dbContext);
    
    public static IColecaoRepository ColecaoRepo(AppDbContext dbContext) =>
        new ColecaoRepository(dbContext);
}
