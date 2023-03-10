using Controls.Net7.Api.Commons.Jwt;
using Controls.Net7.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
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
        //添加JWT请求支持
        c.OperationFilter<AddResponseHeadersFilter>();
        c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
        c.OperationFilter<SecurityRequirementsOperationFilter>();
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
            Name = "Authorization" //JWT默认参数名称
        });
    });
    #endregion
}
builder.Services.AddSingleton<IRedisService, RedisService>(serviceProvider => new RedisService(builder.Configuration.GetSection("redis")["ConnectionString"]));
builder.Services.AddSingleton<ISqliteService, SqliteService>(serviceProvider => new SqliteService("Data Source=db.db"));
#region 注册Jwt验证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    //获取appsettings配置值
    var jwtmodel = builder.Configuration.GetSection(nameof(JwtIssuerOptions));
    var iss = jwtmodel[nameof(JwtIssuerOptions.Issuer)];
    var key = jwtmodel[nameof(JwtIssuerOptions.SecurityKey)];
    var audience = jwtmodel[nameof(JwtIssuerOptions.Audience)];
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true,//是否验证签发者。默认为true。注意，如果设置了TokenValidationParameters.IssuerValidator，则该参数无论是何值，都会执行。
        ValidIssuer = iss,//发行人
        ValidateAudience = true,//有效的受众，即验证Jwt的Payload部分的aud。默认为null
        ValidAudience = audience,//订阅人
        ValidateLifetime = true,//是否验证token是否在有效期内，即验证Jwt的Payload部分的nbf和exp。
        ClockSkew = TimeSpan.Zero,//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
        RequireExpirationTime = true,//是否要求token必须包含过期时间。默认为true，即Jwt的Payload部分必须包含exp且具有有效值。
    };
});
#endregion
#region 授权 那些角色可以访问
builder.Services.AddAuthorization(options =>
{
    // 接口上放[Authorize("admin")] 规定此接口需role为ll和xz才可访问 rolejwt时生成
       options.AddPolicy("授权的人", policy => policy.RequireRole("ll", "xz").Build());
    options.AddPolicy("公共接口", policy => policy.RequireRole("1").Build());

});
#endregion

builder.Services.AddSingleton<IJWTManager, JWTManager>();
//builder.WebHost.UseUrls("http://*:5000");
builder.Services.AddControllers();
var app = builder.Build();
if (useswagger=="true")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseAuthentication();//启用认证1
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
