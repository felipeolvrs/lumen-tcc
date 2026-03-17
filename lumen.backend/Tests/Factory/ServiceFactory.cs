namespace Tests.Factory;

public class ServiceFactory
{ 
    public IProdutoService ProdutoService(AppDbContext ctx)
    {
        var repository = new ProdutoRepository(ctx);
        return new ProdutoService(repository);
    }
    
    public IUsuarioService UsuarioService(AppDbContext ctx)
    {
        var repository = new UsuarioRepository(ctx);
        return new UsuarioService(repository);
    }
    
    public IColecaoService ColecaoService(AppDbContext ctx)
    {
        var repository = new ColecaoRepository(ctx);
        return new ColecaoService(repository);
    }
}