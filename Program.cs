using Microsoft.EntityFrameworkCore;
using PUMP_BACKEND.Data;
using PUMP_BACKEND.Middlewares;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PUMP_BACKEND.Services.Interfaces;
using PUMP_BACKEND.Data.Configurations;

var builder = WebApplication.CreateBuilder(args);

string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins(
            "http://localhost:3000",
            "http://tenant1.localhost:3000",
            "http://tenant2.localhost:3000"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // if needed;
        });
});

//Register the DbContext
builder.Services.AddDbContext<PumpDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var secretKey = builder.Configuration["Jwt:Secret"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey!)),
            ClockSkew = TimeSpan.Zero
        };
    });

//Register DataSeeder
builder.Services.AddTransient<DataSeeder>();

var app = builder.Build();



app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TenantResolutionMiddleware>();

// Seed the database before app.Run()
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    seeder.Seed();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Middleware pipeline
app.UseCors(MyAllowSpecificOrigins);
app.MapGet("/", () => "helloworld!");
app.UseHttpsRedirection();
app.MapControllers();
app.Run("http://0.0.0.0:5164");;