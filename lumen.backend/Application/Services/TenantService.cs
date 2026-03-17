namespace Application.Services;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentTenantId()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
            return Guid.Empty;
        
        string? userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var tenantIdFromToken))
            return tenantIdFromToken;
        
        if (httpContext.Request.Headers.TryGetValue("x-tenant-id", out var tenantIdHeader))
        {
            if (Guid.TryParse(tenantIdHeader.ToString(), out var tenantIdFromHeader))
                return tenantIdFromHeader;
        }

        return Guid.Empty;
    }
}