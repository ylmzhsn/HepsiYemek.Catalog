namespace HepsiYemek.Catalog.Api
{
    // system packages if needed

    // Third party packages
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using HealthChecks.UI.Client;

    // own project references
    using HepsiYemek.Catalog.Data;
    using HepsiYemek.Catalog.Data.Interfaces;
    using HepsiYemek.Catalog.Service.Interfaces;
    using HepsiYemek.Catalog.Service;
    using Microsoft.Extensions.Options;
    using StackExchange.Redis.Extensions.Core.Configuration;
    using StackExchange.Redis.Extensions.Newtonsoft;
    using HepsiYemek.Catalog.Core.CrossCuttingConcerns.Caching.Redis;
    using HepsiYemek.Catalog.Core.CrossCuttingConcerns.Caching.Abstract;

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
            services.Configure<CatalogDatabaseSettings>(
                Configuration.GetSection(nameof(CatalogDatabaseSettings)));

            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<CatalogDatabaseSettings>>().Value);

            services.AddSingleton<RedisServer>();

            services.AddSingleton<ICacheService, RedisCacheService>();

            services.AddScoped<ICatalogContext, CatalogContext>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IProductService, ProductService>();

            services.AddControllers();

            var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();

            services.AddControllersWithViews();

            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(redisConfiguration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HepsiYemek.Catalog.Api", Version = "v1" });
            });

            services.AddHealthChecks()
                    .AddMongoDb(Configuration["CatalogDatabaseSettings:ConnectionString"], "MongoDb Health", HealthStatus.Degraded)
                    .AddElasticsearch(Configuration["ElasticConfiguration:Uri"], "ElasticSearch Health", HealthStatus.Degraded)
                    .AddRedis("127.0.0.1:6379", "Redis Health", HealthStatus.Degraded);                    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HepsiYemek.Catalog.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            app.UseHealthChecksUI();
        }
    }
}
