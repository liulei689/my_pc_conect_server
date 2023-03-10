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
    #region ���swaggerע��
    var basePath = AppContext.BaseDirectory;
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Api"
        });
        var xmlPath = Path.Combine(basePath, "Controls.Net7.Api.xml");// ע������ӿڵ�xml���ƣ�����Ŀ������һ��
        c.IncludeXmlComments(xmlPath, true);
        var xmlDomainPath = Path.Combine(basePath, "Controls.Net7.Api.xml"); // ע������ʵ�����xml���ƣ�����Ŀ������һ��
        c.IncludeXmlComments(xmlDomainPath, true);
        //���JWT����֧��
        c.OperationFilter<AddResponseHeadersFilter>();
        c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
        c.OperationFilter<SecurityRequirementsOperationFilter>();
        c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Description = "JWT��Ȩ(���ݽ�������ͷ�н��д���) ֱ�����¿�������Bearer {token}��ע������֮����һ���ո�",
            Name = "Authorization" //JWTĬ�ϲ�������
        });
    });
    #endregion
}
builder.Services.AddSingleton<IRedisService, RedisService>(serviceProvider => new RedisService(builder.Configuration.GetSection("redis")["ConnectionString"]));
builder.Services.AddSingleton<ISqliteService, SqliteService>(serviceProvider => new SqliteService("Data Source=db.db"));
#region ע��Jwt��֤
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    //��ȡappsettings����ֵ
    var jwtmodel = builder.Configuration.GetSection(nameof(JwtIssuerOptions));
    var iss = jwtmodel[nameof(JwtIssuerOptions.Issuer)];
    var key = jwtmodel[nameof(JwtIssuerOptions.SecurityKey)];
    var audience = jwtmodel[nameof(JwtIssuerOptions.Audience)];
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true,//�Ƿ���֤ǩ���ߡ�Ĭ��Ϊtrue��ע�⣬���������TokenValidationParameters.IssuerValidator����ò��������Ǻ�ֵ������ִ�С�
        ValidIssuer = iss,//������
        ValidateAudience = true,//��Ч�����ڣ�����֤Jwt��Payload���ֵ�aud��Ĭ��Ϊnull
        ValidAudience = audience,//������
        ValidateLifetime = true,//�Ƿ���֤token�Ƿ�����Ч���ڣ�����֤Jwt��Payload���ֵ�nbf��exp��
        ClockSkew = TimeSpan.Zero,//����ǻ������ʱ�䣬Ҳ����˵����ʹ���������˹���ʱ�䣬����ҲҪ���ǽ�ȥ������ʱ��+���壬Ĭ�Ϻ�����7���ӣ������ֱ������Ϊ0
        RequireExpirationTime = true,//�Ƿ�Ҫ��token�����������ʱ�䡣Ĭ��Ϊtrue����Jwt��Payload���ֱ������exp�Ҿ�����Чֵ��
    };
});
#endregion
#region ��Ȩ ��Щ��ɫ���Է���
builder.Services.AddAuthorization(options =>
{
    // �ӿ��Ϸ�[Authorize("admin")] �涨�˽ӿ���roleΪll��xz�ſɷ��� rolejwtʱ����
       options.AddPolicy("��Ȩ����", policy => policy.RequireRole("ll", "xz").Build());
    options.AddPolicy("�����ӿ�", policy => policy.RequireRole("1").Build());

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
app.UseAuthentication();//������֤1
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
