namespace API.Endpoints;

public static class PedidoEndpoints
{
    public static void MapPedidoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/pedidos")
            .WithTags("Pedidos")
            .AllowAnonymous();

        group.MapPost("/criar", async (
                [FromBody] CriarPedidoRequest request,
                [FromServices] IPedidoService service) =>
            {
                try
                {
                    var pedidoCriado = await service.CriarPedidoAsync(request);
                    return Results.Ok(pedidoCriado);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { message = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: "Erro interno ao processar o pedido.", statusCode: 500);
                }
            })
            .WithName("FinalizarPedido");
    }
}