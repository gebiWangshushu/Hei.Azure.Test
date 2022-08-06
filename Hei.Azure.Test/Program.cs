using Hei.Azure.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Passport.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppConfig");
IConfigurationRefresher _refresher = null;

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    ////简单使用只配置connection string
    //config.AddAzureAppConfiguration(connectionString);

    //配置不同功能
    config.AddAzureAppConfiguration(options =>
    {
        //启用Push模式的主动推送更新配置
        options.Connect(connectionString)
            .Select(KeyFilter.Any, LabelFilter.Null)//配置过滤器，读取空Lable的配置
            .Select(KeyFilter.Any, hostingContext.HostingEnvironment.EnvironmentName) //配置过滤器,只读取某个环境的配置
            .ConfigureRefresh(refresh =>
            {
                refresh.Register("TestApp:Settings:Sentinel", refreshAll: true)
                       .SetCacheExpiration(TimeSpan.FromDays(10)); //这个刷新频率要设置特别低了
            });
        _refresher = options.GetRefresher();
    });
});
// Add services to the container.

PassportConfig.InitPassportConfig(builder.Configuration, builder.Environment);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.InjectService();
builder.Services.AddDbContext<UserContext>(options =>
{
    var connectionString = builder.Configuration["CosmosDb:ConnectionString"];
    var databaseName = builder.Configuration["CosmosDb:DatabaseName"];
    options.UseCosmos(connectionString, databaseName);
});

//builder.Services.AddDbContext<UserContext>();

builder.Services.Configure<Settings>(builder.Configuration.GetSection("TestApp:Settings"));
builder.Services.AddAzureAppConfiguration(); //启用Poll模式的主动更新
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//功能开关
//builder.Services.AddFeatureManagement();
//功能开关(过滤器)
builder.Services.AddFeatureManagement()
    .AddFeatureFilter<PercentageFilter>()
    .AddFeatureFilter<TimeWindowFilter>()
    .AddFeatureFilter<CustomFeatureFilter>()
    ;
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAzureAppConfiguration();//启用Poll模式的主动更新
app.UseAzureConfigChangeEventHandler(_refresher);
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();