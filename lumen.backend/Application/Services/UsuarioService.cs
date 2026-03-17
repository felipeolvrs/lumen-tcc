namespace Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<IEnumerable<Usuario>> ObterTodosAsync() 
        => await _usuarioRepository.GetAllAsync();

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        if (id == Guid.Empty) 
            return null;
        
        return await _usuarioRepository.GetByIdAsync(id);
    }

    public async Task<UsuarioResponse?> ObterPerfilPorIdAsync(Guid id)
    {
        var usuario = await ObterPorIdAsync(id);
        return usuario is null ? null : MapToResponse(usuario);
    }
    
    public async Task<UsuarioResponse?> ObterPorSlugAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) 
            return null;

        string slugNormalizado = slug.ToLower().Trim();
    
        var usuario = await _usuarioRepository.ObterPorSlugAsync(slugNormalizado); 

        return usuario is null ? null : MapToResponse(usuario);
    }
    

public async Task<bool> AtualizarAsync(Guid id, AtualizarUsuarioRequest dto)
{
    var usuario = await _usuarioRepository.GetByIdAsync(id);
    
    if (usuario is null)
        return false;

    Usuario? emailEmUso = await _usuarioRepository.GetByEmailAsync(dto.Email);
    
    if (emailEmUso is not null && emailEmUso.Id != id)
        throw new Exception("E-mail já está em uso por outro usuário.");

    usuario.Nome = dto.Nome;
    usuario.Email = dto.Email;

    if (!string.IsNullOrWhiteSpace(dto.Senha))
        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

    if (!string.IsNullOrEmpty(dto.Username))
    {
        string novoSlug = dto.Username.ToLower().Trim().Replace(" ", "-");

        if (usuario.Slug != novoSlug)
        {
            Usuario? slugEmUso = await _usuarioRepository.ObterPorSlugAsync(novoSlug);
            
            if (slugEmUso is not null)
                throw new Exception("Este nome de usuário (slug) já está em uso por outra loja.");

            usuario.Slug = novoSlug;
        }
    }

    usuario.UrlLogo = dto.FotoUrl ?? usuario.UrlLogo;
    usuario.UrlBanner = dto.UrlBanner ?? usuario.UrlBanner;
    usuario.WhatsApp = dto.Whatsapp; 
    usuario.TipoLoja = dto.TipoLoja;
    usuario.Bio = dto.Bio;
    usuario.NomeLoja = dto.NomeLoja;
    usuario.Instagram = dto.Instagram;
    usuario.ChavePix = dto.ChavePix;
    usuario.Ativo = dto.Ativo; 
    usuario.UpdatedAt = DateTime.UtcNow;

    try 
    {
        await _usuarioRepository.UpdateAsync(usuario);
        return true;
    }
    catch (Exception ex)
    {
        throw new Exception("Erro ao salvar no banco. Verifique se a imagem é muito grande.");
    }
}
    public async Task<UsuarioResponse> CriarAsync(CriarUsuarioRequest dto)
    {
        Usuario? existente = await _usuarioRepository.GetByEmailAsync(dto.Email);
        
        if (existente is not null)
            throw new Exception("Este e-mail já está cadastrado.");
        
        var novoUsuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha, 11),
            Ativo = true,
            DataCadastro = DateTime.UtcNow,
            Slug = dto.Nome.ToLower().Trim().Replace(" ", "-"),
            NomeLoja = dto.Nome,
        };

        await _usuarioRepository.AddAsync(novoUsuario);

        return MapToResponse(novoUsuario);
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        if (id == Guid.Empty)
            return false;
        
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        
        if (usuario is null)
            return false;

        await _usuarioRepository.DeleteAsync(usuario.Id);
        return true;
    }
    private UsuarioResponse MapToResponse(Usuario usuario)
    {
        return new UsuarioResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Perfil = usuario.Perfil,
            Ativo = usuario.Ativo,
            DataCadastro = usuario.DataCadastro,
            UpdatedAt = usuario.UpdatedAt,
            Slug = usuario.Slug,
            NomeLoja = usuario.NomeLoja,
            Bio = usuario.Bio,
            UrlLogo = usuario.UrlLogo,
            UrlBanner = usuario.UrlBanner,
            WhatsApp = usuario.WhatsApp,
            Instagram = usuario.Instagram,
            ChavePix = usuario.ChavePix,
            TipoLoja = usuario.TipoLoja 
        };
    }
}