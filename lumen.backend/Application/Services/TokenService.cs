namespace Application.Services;
public class TokenService
{
    private readonly string _jwtKey;

    public TokenService(IConfiguration configuration)
    {
        _jwtKey = configuration["Jwt:Key"] ?? "UmaChaveSuperSecretaDoProjetoVynn123!";
    }
    
    public string GerarToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        byte[] key = Encoding.ASCII.GetBytes(_jwtKey);
        
        List<Claim> claims = new List<Claim>
        {
            // NameIdentifier: É o ID único.
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            
            // Name: O nome para exibir na tela ("Olá, Usuário").
            new Claim(ClaimTypes.Name, usuario.Nome),
            
            // Email: O e-mail do usuário.
            new Claim(ClaimTypes.Email, usuario.Email),
            
            // Role: O Cargo. Define se ele pode entrar na sala da diretoria (Admin) ou não.
            new Claim(ClaimTypes.Role, usuario.Perfil.ToString()),
            
            // Jti: Um número aleatório para garantir que esse token é único.
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}