using UrlShortener.Api;
using UrlShortener.Application;
using UrlShortener.Contracts;
using UrlShortener.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "UrlShortener API v1");
        c.RoutePrefix = ""; // optional, serve Swagger at root /
    });
}
app.MapControllers();
app.Run();