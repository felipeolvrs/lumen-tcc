namespace API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Autenticação") 
            .AllowAnonymous();

        group.MapPost("/registrar", async (
                [FromBody] RegisterRequest request,
                [FromServices] IAuthService service) =>
            {
                try
                {
                    var response = await service.RegistrarAsync(request);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("RegistrarUsuario");

        group.MapPost("/login", async (
            [FromBody] LoginRequest request, 
            [FromServices] IAuthService service) =>
        {
            try
            {
                var response = await service.LoginAsync(request);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("LoginUsuario")
        .WithSummary("Autenticar usuário")
        .WithDescription("Verifica as credenciais e retorna o Token JWT para acesso às áreas restritas.");
    }
}