using Hei.Azure.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppConfig");

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    ////��ʹ��ֻ����connection string
    //config.AddAzureAppConfiguration(connectionString);

    //���ò�ͬ����
    config.AddAzureAppConfiguration(options =>
    {
        ////����Label���໷����֧��
        //options.Connect(connectionString)
        //    .Select(KeyFilter.Any, LabelFilter.Null)//���ù���������ȡ��Lable������
        //    .Select(KeyFilter.Any, hostingContext.HostingEnvironment.EnvironmentName); //���ù�����,ֻ��ȡĳ������������

        //////����Pollģʽ����������
        //options.Connect(connectionString)
        //    .Select(KeyFilter.Any, LabelFilter.Null)//���ù���������ȡ��Lable������
        //    .Select(KeyFilter.Any, hostingContext.HostingEnvironment.EnvironmentName) //���ù�����,ֻ��ȡĳ������������
        //    .ConfigureRefresh(refresh =>
        //    {
        //        refresh.Register("TestApp:Settings:Sentinel", refreshAll: true).SetCacheExpiration(new TimeSpan(0, 0, 30));
        //    });

        ////���ù��ܿ�������
        options.Connect(connectionString)
              .Select(KeyFilter.Any, LabelFilter.Null)//���ù���������ȡ���е�Lable������
              .Select(KeyFilter.Any, hostingContext.HostingEnvironment.EnvironmentName) //���ù�����,ֻ��ȡĳ������������
                                                                                        //���ù��ܿ�������
                                                                                        .UseFeatureFlags(options =>
                                                                                         {
                                                                                             //options.CacheExpirationInterval = TimeSpan.FromMinutes(5); //����FeatureFlag���汾��ʱ��
                                                                                         })
              .ConfigureRefresh(refresh =>
              {
                  refresh.Register("TestApp:Settings:Sentinel", refreshAll: true).SetCacheExpiration(new TimeSpan(0, 0, 30));
              });
    });
});
// Add services to the container.

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
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();