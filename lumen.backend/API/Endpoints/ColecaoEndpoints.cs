namespace API.Endpoints;

public static class ColecaoEndpoints
{
    public static void MapColecaoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/colecoes")
            .WithTags("Coleções (Kits)")
            .SecureEndpoints(); 

        group.MapGet("/", async (IColecaoService service, ClaimsPrincipal user) =>
        {
            return Results.Ok(await service.ObterPorUsuarioAsync(user.GetId()));
        });
        
        group.MapGet("/{id:guid}", async (IColecaoService service, Guid id) =>
        {
            var colecao = await service.ObterPorIdAsync(id);
            return colecao is not null ? Results.Ok(colecao) : Results.NotFound();
        });

        group.MapPost("/", async (IColecaoService service, ClaimsPrincipal user, CriarColecaoRequest request) =>
        {
            var colecao = await service.CriarAsync(user.GetId(), request);
            return Results.Ok(colecao);
        });
        
        group.MapPut("/{id:guid}", async (IColecaoService service, ClaimsPrincipal user, Guid id, AtualizarColecaoRequest request) =>
        {
            await service.AtualizarAsync(id, user.GetId(), request);
            return Results.Ok(new { message = "Coleção atualizada com sucesso" });
        });

        group.MapDelete("/{id:guid}", async (IColecaoService service, Guid id) =>
        {
            await service.ExcluirAsync(id);
            return Results.Ok(new { message = "Coleção removida com sucesso" });
        });


    }
}