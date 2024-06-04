using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => 
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();
builder.Services.AddScoped<ITokenService, TokenService>();
// if we provide an interface inside here, we need to include its implementation class as well 
// We can also use only AddScoped<TokenService> 
// But using interface will help a lot with the testing later 
// It's much easier to test against the interfaces and isolating our code 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
            ValidateIssuer = false, // the issuer of the token is our API server, 
            // but we need to pass that info down w the token in order to validate it
            // we havent implemented that -> say it's false 
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

// app.UseHttpsRedirection();
// if (app.Environment.IsDevelopment())
// {
//     app.UseDeveloperExceptionPage();
// }

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); 
// ask: Do you have a valid token? 
app.UseAuthorization();
// ask: What are you allowed to do? 

app.MapControllers();


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
