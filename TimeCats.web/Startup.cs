using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimeCats.Services;

namespace TimeCats
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var timeTrackerConnectionString = Configuration.GetConnectionString("TimeTrackerDB") ?? throw new ArgumentNullException("Configuration.GetConnectionString(\"TimeTrackerDB\")");

            services.AddDbContext<TimeTrackerContext>(options =>
                options.UseNpgsql(timeTrackerConnectionString));
            services.AddMvc(options => { options.EnableEndpointRouting = false; });
            services.AddSession(options => { options.IdleTimeout = TimeSpan.FromHours(1); });
            services.AddScoped<StudentTimeTrackerService>();
            services.AddScoped<CourseService>();
            services.AddScoped<EvalService>();
            services.AddScoped<GroupService>();
            services.AddScoped<ProjectService>();
            services.AddScoped<TimeService>();
            services.AddScoped<UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
