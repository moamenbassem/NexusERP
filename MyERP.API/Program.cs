
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyERP.Application.Interfaces;
using MyERP.Application.Modules.Account.Interfaces;
using MyERP.Application.Modules.Inventory.Interfaces;
using MyERP.Application.Modules.Purcahsing.Interfaces;
using MyERP.Domain.Entities.Identity;
using MyERP.Infrastructure.Data;
using MyERP.Infrastructure.Modules.Account.Services;
using MyERP.Infrastructure.Modules.Inventory.Services;
using MyERP.Infrastructure.Modules.Purchasing.Services;
using MyERP.Infrastructure.Repositories;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyERP.Application.Modules.AuditLog.Interfaces;
using MyERP.Infrastructure.Modules.AuditLog;
using MyERP.Application.Modules.Finance.Interfaces;
using MyERP.Infrastructure.Modules.Finance;
using MyERP.Application.Modules.CRM.Interfaces;
using MyERP.Infrastructure.Modules.CRM;
using MyERP.Application.Modules.HR.Interfaces;
using MyERP.Infrastructure.Modules.HR;

namespace MyERP.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<AppDbContext>(op => op.UseLazyLoadingProxies().UseSqlServer(
                builder.Configuration.GetConnectionString("MyConnection")
                ));
            // Add services to the container.
            builder.Services.AddIdentity<AppUser, IdentityRole<int>>().AddRoles<IdentityRole<int>>().AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.MaxDepth = 64;
                    // This is the Newtonsoft equivalent of the JsonStringEnumConverter
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                });

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                };
            });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddEndpointsApiExplorer(); // IMPORTANT: Swagger needs this to find your controllers
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddScoped<IPurchasingService, PurchasingService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAuditLogService, AuditLogService>();
            builder.Services.AddScoped<IFinanceService, FinanceService>();
            builder.Services.AddScoped<ICRMService, CRMService>();
            builder.Services.AddScoped<IHRService, HRService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyERP API v1");
                    //c.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
