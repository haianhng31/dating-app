using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {   
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
        // It uses the this keyword before the first parameter to indicate it's an extension method.
        // An extension method is a static method that behaves as if it were an instance method on the type it extends.
        // They allow you to add new methods to existing types without modifying the original type. 
        // 'IServiceCollection services': Specifies that this method extends the IServiceCollection type.
        {
            services.AddDbContext<DataContext>(opt => 
            // AddDbContext: This method registers the DataContext class as a service in the DI container.
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
                // config.GetConnectionString("DefaultConnection"): 
                // Retrieves the connection string from the configuration settings (e.g., appsettings.json) 
                // using the key "DefaultConnection".
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            // if we provide an interface inside here, we need to include its implementation class as well 
            // We can also use only AddScoped<TokenService> 
            // But using interface will help a lot with the testing later 
            // It's much easier to test against the interfaces and isolating our code 

            // The choice of using AddScoped for ITokenService ensures that each HTTP request gets a fresh instance of the service, 
            // which is crucial for handling user-specific tokens securely and efficiently.

            return services; 
        } 

    }
}