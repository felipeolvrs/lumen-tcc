namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly TokenService _tokenService;

    public AuthService(IUsuarioRepository usuarioRepository, TokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegistrarAsync(RegisterRequest request)
    {
        var existente = await _usuarioRepository.GetByEmailAsync(request.Email);
        
        if (existente is not null) 
            throw new Exception("Este e-mail já está cadastrado no sistema.");

        var senhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha,11);

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            Email = request.Email,
            SenhaHash = senhaHash,
            Perfil = Perfil.Usuario,
            DataCadastro = DateTime.UtcNow
        };

        await _usuarioRepository.AddAsync(usuario);
        var token = _tokenService.GerarToken(usuario);
        
        return new AuthResponse(usuario.Id, token, usuario.Nome, usuario.Email);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
            throw new Exception("E-mail ou senha inválidos.");

        var token = _tokenService.GerarToken(usuario);
        
        return new AuthResponse(usuario.Id, token, usuario.Nome, usuario.Email);
    }
}