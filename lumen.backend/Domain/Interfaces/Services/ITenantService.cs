namespace Domain.Interfaces.Services;

public interface ITenantService
{
    Guid GetCurrentTenantId();
}