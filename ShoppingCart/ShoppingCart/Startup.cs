using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingCart.Contracts;
using ShoppingCart.Data;
using ShoppingCart.Data.SqlRepositories;
using ShoppingCart.Entities;
using System;
using System.Reflection;

namespace ShoppingCart
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
            services.AddControllers();
            //Sql Server
            //services.AddDbContext<ShoppingCartDbContext>(options =>
            //     options.UseSqlServer(Configuration.GetConnectionString("ShoppingCartDb")));

            //In Memory Database
            services.AddDbContext<ShoppingCartDbContext>(options =>
            options.UseInMemoryDatabase("ShoppingCartDbContext"));

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddControllersWithViews()
                 .AddNewtonsoftJson(options =>
                         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IOrderItemRepo, OrderItemRepo>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ICustomerRepo, CustomerRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var context = serviceProvider.GetService<ShoppingCartDbContext>();
            AddSeedData(context);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddSeedData(ShoppingCartDbContext context)
        {
            var customer1 = new Customer()
            {
                CustomerName = "Prince Thomas"
            ,
                Address1 = "16, Ashfield"
            ,
                Address2 = "Glencar"
            ,
                Street = "Letterkennny"
            ,
                Town = "Donegal"
            ,
                PostCode = "F92 CK"
            };
            context.Customer.Add(customer1);
            var customer2 = new Customer()
            {
                CustomerName = "Thomas"
           ,
                Address1 = "20, Ashfield"
           ,
                Address2 = "Glencar"
           ,
                Street = "Letterkennny"
           ,
                Town = "Donegal"
           ,
                PostCode = "F92 CK1"
            };
            context.Customer.Add(customer2);

            var product1 = new Product()
            {
                ProductName = "Computer"
            ,
                UnitPrice = 550.00m
                          ,
                Brand = "Dell"
               ,
                TotalStockCount = 30
            };
            context.Product.Add(product1);
            var product2 = new Product()
            {
                ProductName = "TV"
            ,
                UnitPrice = 650.00m
                          ,
                Brand = "Sony"
               ,
                TotalStockCount = 30
            };
            context.Product.Add(product2);
            var product3 = new Product()
            {
                ProductName = "Phone"
            ,
                UnitPrice = 450.00m
                          ,
                Brand = "Samsung"
               ,
                TotalStockCount = 25
            };
            context.Product.Add(product3);
            var product4 = new Product()
            {
                ProductName = "Phone"
            ,
                UnitPrice = 850.00m
                          ,
                Brand = "iPhone"
               ,
                TotalStockCount = 30
            };
            context.Product.Add(product4);

            var order1 = new Order()
            {
                OrderDate = new DateTime(2022, 05, 05)
        ,
                ExpectedDeliveryDate = new DateTime(2022, 05, 25)
                      ,
                CustomerID = 1

            };
            context.SaveChanges();
        }
    }
}
