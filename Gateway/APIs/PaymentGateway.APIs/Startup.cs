using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.ApplicationServices;
using PaymentGateway.Infrastructure;
using PaymentGateway.Infrastructure.Bank;
using PaymentGateway.Infrastructure.Context;
using Swashbuckle.AspNetCore.Filters;

namespace PaymentGateway.APIs
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
            // register services
            services.AddHttpClient();
            services.RegisterAppServices();
            services.RegisterInfraServices();
            services.AddHttpContextAccessor();

            // register options
            services.Configure<MockBankConfigOptions>(Configuration.GetSection(MockBankConfigOptions.MockBankSetting));

            // register db context
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration["ConnectionString:Database"]));

            services.AddControllers();
            
            // configure swagger
            services.AddSwaggerExamplesFromAssemblyOf<Startup>();
            services.AddSwaggerGen(options =>
            {
                options.ExampleFilters();
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentGateway.APIs", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentGateway.APIs v1"));
            }

            if (env.EnvironmentName != "testing")
            {
                dbContext.Database.Migrate();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseGlobalExceptionMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
