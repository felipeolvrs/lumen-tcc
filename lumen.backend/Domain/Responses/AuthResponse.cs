namespace Domain.Responses;

public class AuthResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Token { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public AuthResponse() { }
    
    public AuthResponse(Guid id, string token, string nome, string email)
    {
        Id = id;
        Token = token;
        Nome = nome;
        Email = email;
    }
}