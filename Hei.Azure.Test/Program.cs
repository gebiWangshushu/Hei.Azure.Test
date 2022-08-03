using Hei.Azure.Test;
using Microsoft.EntityFrameworkCore;
using Passport.Infrastructure;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

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
              .UseFeatureFlags() //���ù��ܿ�������
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
builder.Services.AddFeatureManagement(); //���ܿ���
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