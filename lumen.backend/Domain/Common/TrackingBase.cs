namespace Domain.Common;

public class TrackingBase: EntityBase
{
    public DateTime DataOcorrencia { get; set; } = DateTime.UtcNow;
    public string SessionId { get; set; } = Guid.NewGuid().ToString();
    public string UserAgent { get; set; } = string.Empty;
    public string? IpAddress { get; set; } 
    public string? IpHash { get; set; }
    public string? Referrer { get; set; }
    public string Dispositivo { get; set; } = string.Empty;
    public string? Navegador { get; set; }
    public string? SistemaOperacional { get; set; }
    public string? Pais { get; set; }
    public string? Cidade { get; set; }
}