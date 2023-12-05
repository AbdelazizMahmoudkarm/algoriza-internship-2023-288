using algoriza_internship_288.Core.Models;
using algoriza_internship_288.Ef.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.UnitOfWork;
using System.Text.Json.Serialization;

namespace algoriza_internship_288
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            builder.Services.AddDbContext<AppDbContext>(option => option.UseLazyLoadingProxies().
            UseSqlServer(config.GetConnectionString("DefaultDb")));
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddControllers().AddJsonOptions(
                option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
            );

            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure the HTTP request pipeline.
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
           

            app.MapControllers();

            app.Run();
        }
    }
}