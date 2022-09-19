using Quant.DataApi;
using Quant.DataApi.Hubs;
using Quant.Exchanges;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<MarketHub>();
builder.Services.AddSingleton<List<IExchangeQueryClient>>(provider =>
{
    var configuration = provider.GetService<IConfiguration>()!;
    return ExchangeClientFactory.GetQueryExchanges(configuration);
});
builder.WebHost.UseUrls(string.Format("http://*:{0}", builder.Configuration.GetSection("Port").Value));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHub<MarketHub>("/market");
app.UseAuthorization();

app.MapControllers();

app.Run();
