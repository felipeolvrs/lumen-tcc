namespace API.Endpoints;

public static class LojaEndpoints
{
    public static void MapLojaEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/minha-loja")
            .WithTags("Painel do Afiliado (Configuração da Loja)")
            .SecureEndpoints();

        group.MapGet("/", async (ILojaService service, ClaimsPrincipal user) =>
            {
                var config = await service.ObterConfiguracaoAsync(user.GetId());
                return Results.Ok(config ?? new LojaResponse());
            })
            .WithName("ObterMinhaLoja")
            .WithSummary("Recuperar configurações da loja");
        
        group.MapPut("/", async (ILojaService service, ClaimsPrincipal user, LojaRequest dto) =>
        {
            var userId = user.GetId();
            
            if (await service.SlugJaExisteAsync(dto.Slug, userId))
            {
                return Results.BadRequest(new { 
                    error = "Esta URL (slug) já está sendo usada por outra loja. Escolha outra." 
                });
            }

            await service.AtualizarConfiguracaoAsync(userId, dto);
            return Results.Ok(new { message = "Loja atualizada com sucesso!" });
        })
        .WithName("AtualizarMinhaLoja")
        .WithSummary("Atualizar dados da loja")
        .WithDescription("Salva as alterações de identidade visual e URL amigável da loja.");
        
        group.MapGet("/categorias", async (IProdutoService service, ClaimsPrincipal user) =>
        {
            var categorias = await service.ListarCategoriasDoUsuarioAsync(user.GetId());
            return Results.Ok(categorias);
        })
        .WithName("ListarMinhasCategorias")
        .WithSummary("Listar categorias do afiliado")
        .WithDescription("Retorna as categorias de produtos que o afiliado possui em sua vitrine.");
    }
}