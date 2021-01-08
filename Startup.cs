using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ems.Areas.Identity.Data;
using ems.Areas.Identity.Models;
using ems.Data;
using ems.Services;
using ems.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace ems
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(
                        Configuration.GetConnectionString("DbConnection"),
                        new MySqlServerVersion(new Version(
                            int.Parse(Configuration["ConnectionStrings:Database:Major"]),
                            int.Parse(Configuration["ConnectionStrings:Database:Minor"]),
                            int.Parse(Configuration["ConnectionStrings:Database:Build"])
                            )),
                        mySqlOptions => mySqlOptions
                            .CharSetBehavior(CharSetBehavior.NeverAppend))
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
            );
            services.AddControllersWithViews();
            services.Configure<MailSetting>(Configuration.GetSection("MailSetting"));
            services.AddTransient<IMailService, MailService>();
            services.AddIdentity<ApplicationUser, IdentityRole>(option =>
                {
                    option.Password.RequiredLength = 5;
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = false;
                    option.Password.RequireDigit = false;
                }
            )
                .AddEntityFrameworkStores<ApplicationContext>();
            var serviceProvider = services.BuildServiceProvider();
            SampleData.Initialize(serviceProvider);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "area",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
