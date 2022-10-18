using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ZgnWebApi.BackgroundWorkers;
using ZgnWebApi.Core.Extensions;
using ZgnWebApi.Core.Utilities.IoC;
using ZgnWebApi.Core.Utilities.Middlewares;
using ZgnWebApi.Core.Utilities.Security;
using ZgnWebApi.Core.Utilities.Settings;
using ZgnWebApi.DataAccess.Contexts;
using ZgnWebApi.Entities;
using ZgnWebApi.Integrations.BlueBotics;
using ZgnWebApi.Integrations.Klimasan;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication();
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

});
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
    options.JsonSerializerOptions.Converters.Add(new SensitiveConverter());
});
builder.Services.AddCors();
builder.Services.AddSingleton<IBlueBoticsIntegration, BlueBoticsIntegration>();
builder.Services.AddSingleton<IKlimasanIntegration, KlimasanIntegration>();
builder.Services.AddSingleton<ITokenHelper, JwtHelper>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHostedService<TransactionCheckWorkerService>();
ServiceTool.Create(builder.Services);
var origins = builder.Configuration.GetSection("AllowedHosts").Get<string>().Split(';');
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(builder =>
{
    if (origins[0] == "*")
    {
        builder.WithOrigins("*").AllowAnyHeader();
    }
    else
    {
        builder.WithOrigins(origins).AllowAnyHeader();
    }
});
app.ConfigureCustomExceptionMiddleware();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
ZgnAgvManagerContext context = new();
context.Database.Migrate();
try
{
    var u = new User() { Type="Admin", FirstName = "Technolife", LastName = "", Password = "123456", Status = true, UserName = "Supervisor" };
    u.CheckAndAdd();
}
catch (Exception){}
app.Run();
