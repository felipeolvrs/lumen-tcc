namespace Infrastructure;

public class AppDbContext : DbContext
{
    private readonly ITenantService? _tenantService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService? tenantService = null) 
        : base(options)
    {
        _tenantService = tenantService;
    }

    public Guid CurrentTenantId 
    {
        get 
        {
            try
            {
                return _tenantService?.GetCurrentTenantId() ?? Guid.Empty;
            }
            catch
            {
                return Guid.Empty;
            }
        }
    }

    public DbSet<Produto> Produtos { get; set; }
    public DbSet<PedidoItem> PedidoItens { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<LojaConfig> LojaConfigs { get; set; }
    public DbSet<Colecao> Colecoes { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(100);
        configurationBuilder.Properties<decimal>().HavePrecision(10, 2);
        configurationBuilder.Properties<Guid>().HaveColumnType("char(36)");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(builder =>
        {
            builder.Property(u => u.SenhaHash).HasMaxLength(255);
            builder.Property(u => u.Bio).HasMaxLength(500);
            builder.Property(u => u.UrlLogo).HasColumnType("longtext"); 
            builder.Property(u => u.UrlBanner).HasColumnType("longtext");
            builder.Property(u => u.Slug).HasMaxLength(100);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Slug).IsUnique();
        });

        modelBuilder.Entity<Produto>(builder =>
        {
            builder.Property(p => p.Descricao).HasMaxLength(2000);
            builder.Property(p => p.ReviewPessoal).HasMaxLength(1000);
            builder.Property(p => p.UrlImagem).HasColumnType("longtext");
            
            builder.HasOne(p => p.Usuario).WithMany(u => u.Produtos).HasForeignKey(p => p.TenantId);
            builder.HasOne(p => p.Colecao).WithMany(c => c.Produtos).HasForeignKey(p => p.ColecaoId);
            builder.HasQueryFilter(p => p.TenantId == CurrentTenantId);
        });

        modelBuilder.Entity<Colecao>(builder =>
        {
            builder.Property(c => c.Descricao).HasMaxLength(1000);
            builder.Property(c => c.UrlCapa).HasColumnType("longtext");
            
            builder.HasQueryFilter(c => c.TenantId == CurrentTenantId);
        });

        modelBuilder.Entity<Pedido>(builder =>
        {
            builder.HasMany(p => p.Itens)
                   .WithOne()
                   .HasForeignKey(pi => pi.PedidoId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasQueryFilter(p => p.TenantId == CurrentTenantId);
        });

        modelBuilder.Entity<LojaConfig>().HasQueryFilter(l => l.TenantId == CurrentTenantId);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<EntityBase>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
                entry.Entity.Ativo = true;
                
                if (entry.Entity.TenantId == Guid.Empty) 
                    entry.Entity.TenantId = CurrentTenantId;
            }
            if (entry.State == EntityState.Modified) 
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}