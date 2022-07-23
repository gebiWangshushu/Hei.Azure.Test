using Hei.Azure.Test;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppConfig");

builder.Host.ConfigureAppConfiguration(builder =>
{
    //Connect to your App Config Store using the connection string
    builder.AddAzureAppConfiguration(options =>
    {
        options.Connect(connectionString)
               .ConfigureRefresh(refresh =>
               {
                   refresh.Register("TestApp:Settings:Sentinel", refreshAll: true).SetCacheExpiration(new TimeSpan(0, 0, 30));
               });
    });
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.InjectService();
builder.Services.Configure<Settings>(builder.Configuration.GetSection("TestApp:Settings"));
builder.Services.AddAzureAppConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAzureAppConfiguration();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();