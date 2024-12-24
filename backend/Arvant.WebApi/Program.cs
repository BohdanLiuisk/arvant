using Arvant.Entity.Extensions;
using Arvant.Infrastructure;
using Arvant.Infrastructure.Context;
using Serilog;
using Arvant.WebApi.Extensions;
using Arvant.WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices(builder.Configuration);
builder.Services.AddArvantAuthentication();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

await app.InitialiseDatabaseAsync();
app.UseArvantEntity();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ArvantHub>("/hubs/arvant");
app.MapHub<CallHub>("/hubs/call");
app.Run();
