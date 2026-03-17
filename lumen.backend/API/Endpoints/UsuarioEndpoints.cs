namespace API.Endpoints;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/usuarios")
            .WithTags("Usuários");

        group.MapGet("/me", async (ClaimsPrincipal user, IUsuarioService service) =>
        {
            var usuario = await service.ObterPerfilPorIdAsync(user.GetId()); 
            return usuario is not null ? Results.Ok(usuario) : Results.NotFound();
        }).WithName("ObterPerfilLogado");

        group.MapPut("/me", async (ClaimsPrincipal user, [FromBody] AtualizarUsuarioRequest request, IUsuarioService service) =>
        {
            var sucesso = await service.AtualizarAsync(user.GetId(), request);
            return sucesso ? Results.Ok(new { message = "Perfil atualizado!" }) : Results.BadRequest(new { error = "Erro ao atualizar perfil" });
        }).WithName("AtualizarPerfilLogado");
        
        group.MapGet("/{id:guid}", async (Guid id, IUsuarioService service) =>
        {
            var usuario = await service.ObterPerfilPorIdAsync(id);
            return usuario is null ? Results.NotFound() : Results.Ok(usuario);
        });

        group.MapDelete("/{id:guid}", async (Guid id, IUsuarioService service) =>
        {
            var deleted = await service.ExcluirAsync(id);
            return deleted ? Results.Ok(new { message = "Removido com sucesso" }) : Results.NotFound();
        });
        
        var vitrineGroup = app.MapGroup("/api/usuarios/vitrine")
            .WithTags("Vitrine Pública");

        vitrineGroup.MapGet("/{slug}", async (string slug, IUsuarioService service) =>
        {
            var loja = await service.ObterPorSlugAsync(slug);
            return loja is not null ? Results.Ok(loja) : Results.NotFound();
        }).WithName("ObterVitrinePorSlug");
    }
}