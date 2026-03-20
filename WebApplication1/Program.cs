using ConfigService.Database;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Inputs;
using WebApplication1.Service;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationContext")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Repositories
            builder.Services.AddScoped<IEntityRepository<Actor, Guid>, ActorRepository>();
            builder.Services.AddScoped<IEntityRepository<Document, Guid>, DocumentRepository>();

            //Services

            builder.Services.AddScoped(typeof(IService<DocumentCreateInput, DocumentUpdateInput, Document, Guid>), typeof(DocumentService));
            //builder.Services.AddScoped<IService<DocumentCreateInput, DocumentUpdateInput, Document, Guid>, DocumentService>();
            //builder.Services.AddScoped<IDocumentService, DocumentService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
           
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
