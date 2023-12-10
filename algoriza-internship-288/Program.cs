using algoriza_internship_288.Domain.Models;
using algoriza_internship_288.Repository.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.UnitOfWork;
using System.Text;
using System.Text.Json.Serialization;

namespace algoriza_internship_288
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //  IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            ConfigurationManager config = builder.Configuration;
            builder.Services.AddDbContext<AppDbContext>(option => option.UseLazyLoadingProxies().
            UseSqlServer(config.GetConnectionString("DefaultDb")));
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddControllers().AddJsonOptions(
                option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
            );

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                 option =>
                 {
                     option.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateLifetime = true,
                         ValidateAudience = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = config["Jwt:Issuer"],
                         ValidAudience = config["Jwt:Audience"],
                         
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                     };
                 });
    //        {
    //            "GoogleUser": {
    //                "ClientId": "682273977585-qsmhtmcvstlkj7hcgqqsmbn3t9fqu6ui.apps.googleusercontent.com",
    //                "ClientSecret": "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"
    //            }
    //        }
            builder.Services.AddAuthentication().AddGoogle(option =>
            {//the data comes from secret file
                option.ClientId = config["GoogleUser:ClientId"];
                option.ClientSecret = config["GoogleUser:ClientSecret"];
            });
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