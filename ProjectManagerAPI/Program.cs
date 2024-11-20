using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProjectManagerAPI.Data;
using ProjectManagerAPI.Services;
using ProjectManagerAPI.Utils;
using ProjectManagerAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Serviços
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddSingleton(new JwtHelper(builder.Configuration["Jwt:Secret"]));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Autenticação e Autorização
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Regular", policy => policy.RequireRole("Regular"));
});

var app = builder.Build();

// Configuração do Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware de Autenticação Personalizado
app.Use(async (context, next) =>
{
    if (context.Request.Headers.ContainsKey("Authorization"))
    {
        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var jwt = app.Services.GetRequiredService<JwtHelper>();
        if (jwt.ValidateToken(token, out var payload))
        {
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, payload["sub"].ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, payload["email"].ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, payload["role"].ToString())
            };
            var identity = new System.Security.Claims.ClaimsIdentity(claims, "jwt");
            context.User = new System.Security.Claims.ClaimsPrincipal(identity);
        }
    }
    await next();
});

// Autenticação e Autorização Padrão
app.UseAuthentication();
app.UseAuthorization();

// Mapear Endpoints
app.MapUserEndpoints();
app.MapProjectEndpoints();
app.MapTaskEndpoints();

app.Run();
