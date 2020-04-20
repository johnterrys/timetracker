using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            var connectionstring = Configuration["ConnectionString:TimeTrackerDB"];

            Console.WriteLine($"CONFIGURATION: {Configuration}");
            Console.WriteLine($"POSTGRES_CONNECTION_STRING: {Configuration.GetConnectionString("TimeTrackerDB")}");
            
            services.AddDbContext<TimeTrackerContext>(options =>
                options.UseNpgsql(connectionstring));
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
