
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MinimalApiAuthentication;
using MinimalApiAuthentication.Models;
using MinimalApiAuthentication.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=Test.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "My API", Description = "My API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Jwt Authentication",
        Description = "Jwt Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddJwtBearerAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/Login", async (UserViewModel user, IAuthService authService) =>
{
    var result = await authService.LoginAsync(user.Email, user.Password);
    if (result.Succeeded)
        return Results.Ok(result.Token);

    return Results.BadRequest(result.Errors);
});

app.MapPost("/Register", async (UserViewModel user, IAuthService authService) =>
{
    var result = await authService.RegisterAsync(user.Email, user.Password);
    if (result.Succeeded)
        return Results.Ok(result.Token);

    return Results.BadRequest(result.Errors);
});

app.MapGet("/", () => "Hello World!");
app.MapGet("/Secured", () => "Secured").RequireAuthorization();

app.Run();
