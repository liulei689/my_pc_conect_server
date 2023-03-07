using Controls.Net7.Api.Redis;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.WebHost.UseUrls("http://*:5000");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
string? usecomnets =builder.Configuration.GetSection("Configs")["UseComments"];
string? useswagger = builder.Configuration.GetSection("Configs")["UseSwagger"];
if (usecomnets == "false") builder.Services.AddSwaggerGen();
else
{
    #region 添加swagger注释
    var basePath = AppContext.BaseDirectory;
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Api"
        });
        var xmlPath = Path.Combine(basePath, "Controls.Net7.Api.xml");// 注意这里接口的xml名称，与项目名保持一致
        c.IncludeXmlComments(xmlPath, true);
        var xmlDomainPath = Path.Combine(basePath, "Controls.Net7.Api.xml"); // 注意这里实体类的xml名称，与项目名保持一致
        c.IncludeXmlComments(xmlDomainPath, true);
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Value: Bearer {token}",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
      {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference
          {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          },Scheme = "oauth2",Name = "Bearer",In = ParameterLocation.Header,
        },new List<string>()
      }
        });
    });
    #endregion
}
builder.Services.AddSingleton<IRedisService, RedisService>(serviceProvider => new RedisService(builder.Configuration.GetSection("redis")["ConnectionString"]));
var app = builder.Build();
if (useswagger=="true")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
