namespace API.Extensions;

public static class ClaimsExtensions
{
    public static Guid GetId(this ClaimsPrincipal user)
    {
        var idTexto = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
       
        if (!Guid.TryParse(idTexto, out var id))
            throw new Exception("Não foi possível identificar o ID do usuário no Token."); 
        
        return id;
    }
    
}