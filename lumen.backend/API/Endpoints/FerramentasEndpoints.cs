namespace API.Endpoints;

public static class FerramentasEndpoints
{
    public static void MapFerramentasEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/ferramentas")
            .WithTags("Ferramentas Omnichannel");

        group.MapPost("/checkout-whatsapp", ([FromBody] GerarLinkCheckoutRequest request, [FromServices] IFerramentasService service) =>
        {
            try 
            {
                var response = service.GerarLinkCheckoutWhatsapp(request);
                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }).WithName("GerarLinkCheckoutWhatsapp");

        group.MapGet("/qrcode", ([FromQuery] string url, [FromServices] IFerramentasService service) =>
        {
            try 
            {
                var qrCodeBytes = service.GerarQrCode(url);
                return Results.File(qrCodeBytes, "image/png");
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }).WithName("GerarQrCode");

        group.MapGet("/whatsapp", ([FromQuery] string numero, [FromQuery] string msg, [FromServices] IFerramentasService service) =>
        {
            try 
            {
                var linkFinal = service.GerarLinkWhatsapp(numero, msg);
                return Results.Ok(new { link = linkFinal });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }).WithName("GerarLinkWhatsapp");
        
        group.MapGet("/pix", (string chave, string nome, string cidade, decimal? valor, [FromServices] IFerramentasService service) =>
        {
            try 
            {
                var copiaECola = service.GerarPixEstatico(chave, nome, cidade, valor, "lumen");
                return Results.Ok(new { pixCopiaECola = copiaECola });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        }).WithName("GerarPix");
    }
}