namespace Application.Services;

public class LojaService : ILojaService
{
    private readonly ILojaRepository _lojaRepository;

    public LojaService(ILojaRepository lojaRepository)
    {
        _lojaRepository = lojaRepository;
    }

    public async Task<LojaResponse?> ObterConfiguracaoAsync(Guid userId)
    {
        var loja = await _lojaRepository.GetByUserIdAsync(userId);
        
        if (loja is null)
            return null;

        return new LojaResponse
        {
            Nome = loja.NomeLoja,
            Slug = loja.Slug,
            BannerUrl = loja.UrlBanner
        };
    }
    
    public async Task<bool> SlugJaExisteAsync(string slug, Guid userId)
    {
        string slugNormalizado = slug.ToLower().Trim();
        return await _lojaRepository.AnyWithSlugAsync(slugNormalizado, userId);
    }
    
    public async Task AtualizarConfiguracaoAsync(Guid userId, LojaRequest request)
    {
        var loja = await _lojaRepository.GetByUserIdAsync(userId);

        if (loja is null)
        {
            loja = new LojaConfig
            {
                Id = Guid.NewGuid(),
                UsuarioId = userId,
                DataCriacao = DateTime.UtcNow
            };
        }

        loja.NomeLoja = request.Nome;
        loja.Slug = request.Slug.ToLower().Trim();
        loja.UrlBanner = request.BannerUrl;
        loja.UltimaAtualizacao = DateTime.UtcNow;
        
        

        if (!await _lojaRepository.ExistsAsync(loja.Id))
        {
            await _lojaRepository.AddAsync(loja);
            return;
        }
        
        await _lojaRepository.UpdateAsync(loja);

    }
}