using API.Data;
using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// WebApplication: This is a class provided by ASP.NET Core to create a web application.
// CreateBuilder(args): This method initializes a new instance of the WebApplicationBuilder class.
// The WebApplicationBuilder class helps in configuring services, middlewares, and the application itself.

// Add services to the container.
// builder.Services: This is an instance of IServiceCollection 
// which is used to register services with the dependency injection (DI) container.

builder.Services.AddControllers(); // adds the necessary services for controllers to the service collection.
// It sets up MVC (Model-View-Controller) and API-related services.
// This includes routing, model binding, validation, and other features needed to handle HTTP requests using controllers.

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

app.UseHttpsRedirection();
// This middleware ensures that all HTTP requests are redirected to their HTTPS equivalent. 
// It's like making sure that whenever you're going somewhere, you're taking the safest route possible.

app.UseRouting(); // sets up routing for incoming requests, 
// Routing ensures that each request is directed to the appropriate endpoint or controller within the application.

app.UseAuthentication(); 
// ask: Do you have a valid token? 
// checks whether incoming requests are associated with authenticated users, 
// meaning users who have logged in or provided valid credentials.

app.UseAuthorization();
// ask: What are you allowed to do? 
// Authorization controls what authenticated users can do within the application.

app.MapControllers(); // maps HTTP requests to controller actions within the application.
// Controllers are the components responsible for processing incoming requests, executing the necessary logic, and producing responses. 
// Mapping requests to controllers allows the application to handle different types of requests efficiently. 

using var scope = app.Services.CreateScope(); // gives us access to all the services that we have inside this program class
var services = scope.ServiceProvider;
try {
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUser(context);
} catch (Exception ex){
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "An Error occured during migration");
}

app.Run();
