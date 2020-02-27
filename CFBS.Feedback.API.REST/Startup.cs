using System;
using AutoMapper;
using CFBS.Feedback.API.REST.Models;
using CFBS.Feedback.API.REST.Services.Implementations;
using CFBS.Feedback.DAL;
using CFBS.Feedback.DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CFBS.Feedback.API.REST
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper(typeof(Startup));

            if (Environment.IsDevelopment())
            {
                services.AddDbContext<FeedbackContext>(options => options.UseLazyLoadingProxies()
                    .UseInMemoryDatabase("Feedback"));
            }
            else
            {
                services.AddDbContext<FeedbackContext>(options => options.UseLazyLoadingProxies()
                    .UseSqlServer("Server=tcp:cfbs.database.windows.net,1433;Initial Catalog=cfbs;Persist Security Info=False;User ID=S6059168;Password=Sm258469713;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
            }

            services.AddScoped<ActiveQuestionRepository>();
            services.AddScoped<AnswerRepository<ImageAnswerDetailsDTO>>();
            services.AddScoped<AnswerRepository<AnswerDetailsDTO>>();
            services.AddScoped<ImageAnswerRepository>();
            services.AddScoped<ImageRepository>();
            services.AddScoped<LocationRepository>();
            services.AddScoped<QuestionRepository>();
            services.AddScoped<SubmittedImageAnswerRepository>();
            services.AddScoped<SubmittedTextAnswerRepository>();
            services.AddScoped<TextAnswerRepository>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Feedback REST API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Feedback REST API");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
