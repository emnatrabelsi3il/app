using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PR3_API.Models;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Api",
        Description = "Secures API using JWT",
       TermsOfService = new Uri("https://example.com/terms"),
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()

    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "3iL",
        ValidAudience = "API Test",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("monSuperSecretmonSuperSecretmonSuperSecretmonSuperSecret")),
        ClockSkew = TimeSpan.Zero // Optional: Set to zero to test exact token expiry
    };
});




builder.Services.AddControllers();
builder.Services.AddDbContext<SalleContext>(options => options.UseSqlite("DataSource=PR3-database.db"));
builder.Services.AddDbContext<PosteContext>(options => options.UseSqlite("DataSource=PR3-database.db"));
builder.Services.AddDbContext<UtilisateurContext>(options => options.UseSqlite("DataSource=PR3-database.db"));
builder.Services.AddDbContext<EtablissementContext>(options => options.UseSqlite("DataSource=PR3-database.db"));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
