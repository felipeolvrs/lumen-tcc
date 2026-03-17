namespace Domain.Common;

public abstract class EntityBase : TenantEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool Ativo { get; set; } = true;
    protected EntityBase(){}
}