using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using EFCoreNewDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace EFCoreNewDatabase
{
    public class Startup
    {
        // this class needs the connection info stored in the 
        // appsettings.json config file - that's a dependency
        // with dependency injection we expose the config file to this class
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:BlogIdentity:ConnectionString"]));
            services.AddIdentity<AppUser, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.Password.RequiredUniqueChars = 1;
            }).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
            // this is where we use the config info for our connection string
            services.AddDbContext<BloggingContext>(options => options.UseSqlServer(Configuration["Data:Blog:ConnectionString"]));
            // since we created an interface for our repository, we must map the 
            // interface to the concrete class to ensure that when an IBloggingRepository
            // is requested, a new instance of EFBloggingRepository is returned
            services.AddTransient<IBloggingRepository, EFBloggingRepository>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Make the authentication service available to the application (Order matters here)
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            // TODO: remove after database has been seeded
            // DO NOT deploy to production without removing this line
            //SeedData.EnsurePopulated(app);
        }
    }
}
