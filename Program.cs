using Microsoft.EntityFrameworkCore;
using VocaSRS.Context;
using VocaSRS.Services;

namespace VocaSRS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IVocaService, VocaService>();
            builder.Services.AddDbContext<VocaContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("VocaSrsConnection")));

            WebApplication app = builder.Build();

            CreateDbIfNotExists(app);

            app.UseStaticFiles();

            app.UseRouting();
            app.UseDeveloperExceptionPage();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                try
                {
                    VocaContext context = services.GetRequiredService<VocaContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
    }
}