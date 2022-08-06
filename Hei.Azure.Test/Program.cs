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
    ////��ʹ��ֻ����connection string
    //config.AddAzureAppConfiguration(connectionString);

    //���ò�ͬ����
    config.AddAzureAppConfiguration(options =>
    {
        //����Pushģʽ���������͸�������
        options.Connect(connectionString)
            .Select(KeyFilter.Any, LabelFilter.Null)//���ù���������ȡ��Lable������
            .Select(KeyFilter.Any, hostingContext.HostingEnvironment.EnvironmentName) //���ù�����,ֻ��ȡĳ������������
            .ConfigureRefresh(refresh =>
            {
                refresh.Register("TestApp:Settings:Sentinel", refreshAll: true)
                       .SetCacheExpiration(TimeSpan.FromDays(10)); //���ˢ��Ƶ��Ҫ�����ر����
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
builder.Services.AddAzureAppConfiguration(); //����Pollģʽ����������
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//���ܿ���
//builder.Services.AddFeatureManagement();
//���ܿ���(������)
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

app.UseAzureAppConfiguration();//����Pollģʽ����������
app.UseAzureConfigChangeEventHandler(_refresher);
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();