using ApiGatewayBalanceador.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<LoadBalancer>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
