var builder = WebApplication.CreateBuilder(args);

builder.AddArchitecture();
builder.Services.AddMyServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("PermitirTudo");
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapAllApiEndpoints(); 
app.Run();