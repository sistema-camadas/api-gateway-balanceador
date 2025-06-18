using ApiGatewayBalanceador.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<LoadBalancer>();
builder.Services.AddHttpClient("Insecure")
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
