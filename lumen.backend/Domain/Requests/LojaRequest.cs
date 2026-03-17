namespace Domain.Requests;

public class LojaRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? BannerUrl { get; set; }
}