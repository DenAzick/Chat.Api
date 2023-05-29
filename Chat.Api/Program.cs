using Identity.Core.Context;
using Identity.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Identity.Core.Middlewares;
using Chat.Core.Context;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Bearer. : \"Authorization: Bearer { token }\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<ChatDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ChatDb"));
});

builder.Services.AddDbContext<IdentityDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("IdentityDb"));
});

builder.Services.AddIdentity(builder.Configuration);

//builder.Services.AddScoped<Conversation>

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseCors(cors =>
{
    cors.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
}); 


//app.MigrateIdentityDb(this WebApplication app);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.UseErrorHandlerMiddleware();

app.MapControllers();

app.Run();
