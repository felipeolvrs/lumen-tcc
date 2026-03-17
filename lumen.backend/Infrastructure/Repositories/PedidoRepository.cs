namespace Infrastructure.Repositories;

public class PedidoRepository : RepositoryBase<Pedido>, IPedidoRepository
{
    public PedidoRepository(AppDbContext context) : base(context)
    {
    }
}