using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TagStore.Service.Data;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using TagStore.Service.Data.Items;
using Swashbuckle.AspNetCore.Swagger;
//using Microsoft.OData.Edm;
//using Microsoft.AspNet.OData.Builder;
using TagStore.Service.Models.Items;
//using Microsoft.AspNet.OData.Extensions;

namespace TagStore.Service
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
            services.AddDbContext<Data.Items.ItemsContext>(o => o.UseSqlite("Data Source=tagStoreItems.db"));
            //services.AddDbContext<Data.Items.ItemsContext>(o => o.UseSqlServer(@"Server=.\SQLExpress;Database=tagStoreItems;Trusted_Connection=Yes;"));
            //services.AddDbContext<Data.Items.ItemsContext>(o => o.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ShirtDB;Trusted_Connection=True;MultipleActiveResultSets=true"));

            services.AddScoped<IItemsRepository, ItemsRepository>();
            //services.AddOData();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TagStore API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Data.Items.ItemsContext itemsContext)
        {
            Data.Items.DbInitializer.Initialize(itemsContext);
#if DEBUG
            if (env.IsDevelopment())
                Data.Items.DbInitializer.SeedTestingData(itemsContext);
#endif

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TagStore API V1");
                c.RoutePrefix = string.Empty; 
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc(b =>
            {
                //b.MapODataServiceRoute("odata", "odata", GetEdmModel());
            });
        }

        // odata access not working
        // see: https://devblogs.microsoft.com/odata/asp-net-core-odata-now-available/
        //static IEdmModel GetEdmModel()
        //{
        //    var builder = new ODataConventionModelBuilder();
        //    builder.EntitySet<Item>("Item");            
        //    builder.EntitySet<Models.Items.Tag>("Tag");
        //    builder.EntitySet<TagType>("TagType");
        //    builder.EntitySet<TagTypeName>("TagTypeName");
        //    return builder.GetEdmModel();
        //}
    }
}
