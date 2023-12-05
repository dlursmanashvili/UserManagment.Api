using Application.Shared;
using Infrastructure.Db; // Make sure to add this line
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore; // Make sure to add this line
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("appsettings.json");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
DI.DependecyResolver(builder.Services);

builder.Services.AddControllers();
builder.Services.ConfigureSwagger(builder.Configuration["Swagger:Title"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "IssuerTEST",
        ValidAudience = "AudienceTest",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("N1CJGESHJSXETWXL493")),
        ClockSkew = TimeSpan.Zero
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseCors("AllowOrigin");
var enableSwaggerOnlyDevelopment = Convert.ToBoolean(builder.Configuration["Swagger:OnlyDevelopment"]);

if (app.Environment.IsDevelopment())
{
    app.StartSwagger(builder.Configuration["Swagger:Title"], "/swagger/v1/swagger.json");
}
else
{
    if (!enableSwaggerOnlyDevelopment)
        app.StartSwagger(builder.Configuration["Swagger:Title"]);
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();



app.MapControllers();

app.Run();
