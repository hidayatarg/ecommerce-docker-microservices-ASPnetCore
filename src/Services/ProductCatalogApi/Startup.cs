﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductCatalogApi.Data;
using Swashbuckle.AspNetCore.Swagger;


namespace ProductCatalogApi
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
            services.Configure<CatalogSettings>(Configuration);
//          services.AddDbContext<CatalogContext>(options => options.UseNpgsql(Configuration.GetConnectionString("ProductCatalog")));
//          UPDATED With Docker Container
//          Testings and development
            var server = Configuration["DatabaseServer"] ?? "localhost";
            var database = Configuration["DatabaseName"] ?? "CatalogDB";
            var user = Configuration["DatabaseUser"] ?? "sa";
            var password = Configuration["DatabasePassword"] ?? "ProductApi(!)";
            var connectionString =
                string.Format($"Server={server};Database={database};User ID={user};Password={password};");
            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(connectionString));

            services.AddMvc();
//          adding swagger documentation API
//          register swagger
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "ShoesOnContainers - Product Catalog HTTP API",
                    Version = "v1",
                    Description = "The Product Catalog Micro-service HTTP API. This is a Data-Driven/CRUD micro-service sample",
                    TermsOfService = "Terms Of Service"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
//          swagger endpoints
            app.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/v1/swagger.json", "ProductCatalogAPI V1"); });
            app.UseMvc();
        }
    }
}
