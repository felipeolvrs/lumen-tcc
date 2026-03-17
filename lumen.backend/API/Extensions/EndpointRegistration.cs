namespace API.Extensions;

public static class EndpointRegistration
{
    public static void MapAllApiEndpoints(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        app.MapGet("/", () => Results.Redirect("/swagger"));
        app.MapAuthEndpoints();
        app.MapUsuarioEndpoints();
        app.MapProdutoEndpoints();
        app.MapPedidoEndpoints();
        app.MapColecaoEndpoints();
        app.MapFerramentasEndpoints();
    }
}