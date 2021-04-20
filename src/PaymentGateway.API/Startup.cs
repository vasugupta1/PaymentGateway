using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PaymentGateway.API.Models;
using PaymentGateway.API.Models.ApiConfiguration;
using PaymentGateway.Common.Models.Storage;
using PaymentGateway.Services.AuthUsers;
using PaymentGateway.Services.AuthUsers.Interface;
using PaymentGateway.Services.Banking;
using PaymentGateway.Services.Banking.Interface;
using PaymentGateway.Services.PaymentProcessor;
using PaymentGateway.Services.PaymentProcessor.Interface;
using PaymentGateway.Services.Storage;
using PaymentGateway.Services.Storage.Interface;
using Refit;
using StackExchange.Redis;

namespace PaymentGateway.API
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
            
            var configObject = GetConfigObject();
            
            services.AddControllers();

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddScoped<IBankingRefitServiceProvider>(x => RestService.For<IBankingRefitServiceProvider>(configObject.Bank.Url));
            services.AddScoped<IBankingService, BankingService>();
            services.AddScoped<IStorageService<PaymentAudit>, StorageService<PaymentAudit>>();
            services.AddScoped<IPaymentProcessorService, PaymentProcessorService>();
            services.AddScoped<IUserService>(x => new UserService(configObject.Authentication));
            services.AddSingleton(ConnectionMultiplexer.Connect(configObject.RedisSettings.ConnectionString).GetDatabase());

            services.AddApiVersioning(config => {
                 // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the API versions supported for the particular endpoint
                config.ReportApiVersions = true;
            });

            services.AddSwaggerGen(x => 
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Gateway", Version = "v1" });
            });
        }

        private CustomConfiguration GetConfigObject()
        {
            var configObject = new CustomConfiguration();
            Configuration.GetSection(nameof(CustomConfiguration)).Bind(configObject);
            return configObject;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 API");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
