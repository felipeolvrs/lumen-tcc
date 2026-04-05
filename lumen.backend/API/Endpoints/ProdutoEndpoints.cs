namespace API.Endpoints;

public static class ProdutoEndpoints
{
    public static void MapProdutoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/produtos")
            .WithTags("Produtos")
            .SecureEndpoints();

        group.MapGet("/", async (IProdutoService service) =>
        {
            var produtos = await service.ObterTodosAsync();
            return Results.Ok(produtos);
        });

        group.MapGet("/{id:guid}", async (IProdutoService service, Guid id) =>
        {
            var produto = await service.ObterPorIdAsync(id);
            return produto is not null ? Results.Ok(produto) : Results.NotFound();
        });

        group.MapPost("/", async (IProdutoService service, ClaimsPrincipal user, ProdutoRequest request) =>
        {
            var produto = await service.CriarAsync(user.GetId(), request);
            return Results.Created($"/api/produtos/{produto.Id}", produto);
        });
        
        group.MapPost("/{id:guid}/ativar", async (IProdutoService service, ClaimsPrincipal user, Guid id) =>
        {
            var sucesso = await service.UpdateStatusAsync(id, true, user.GetId());
    
            return sucesso 
                ? Results.Ok(new { message = "Produto ativado com sucesso" }) 
                : Results.Json(new { message = "Produto não encontrado ou acesso negado" }, statusCode: 403);
        });
        
        group.MapPost("/{id:guid}/desativar", async (IProdutoService service, ClaimsPrincipal user, Guid id) =>
        {
            var sucesso = await service.UpdateStatusAsync(id, false, user.GetId());
    
            return sucesso 
                ? Results.Ok(new { message = "Produto desativado com sucesso" }) 
                : Results.Json(new { message = "Produto não encontrado ou acesso negado" }, statusCode: 403);
        });

        group.MapPut("/{id:guid}", async (IProdutoService service, ClaimsPrincipal user, Guid id, ProdutoRequest request) =>
        {
            var sucesso = await service.AtualizarAsync(id, user.GetId(), request);
            
            return sucesso 
                ? Results.Ok(new { message = "Produto atualizado com sucesso" }) 
                : Results.Json(new { message = "Produto não encontrado ou acesso negado" }, statusCode: 403);
        });

        group.MapDelete("/{id:guid}", async (IProdutoService service, ClaimsPrincipal user, Guid id) =>
        {
            var sucesso = await service.ExcluirAsync(id, user.GetId());
            
            return sucesso 
                ? Results.Ok(new { message = "Produto removido com sucesso" }) 
                : Results.Json(new { message = "Produto não encontrado ou acesso negado" }, statusCode: 403);
        });
    }
}