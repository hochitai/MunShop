using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using MunShopApplication.Repository;
using MunShopApplication.Repository.SQLServer;
using MunShopApplication.Services;

namespace MunShopApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<SqlConnection>(sp =>
            {
                var connectionString = builder.Configuration.GetConnectionString("ShopDatabase");
                return new SqlConnection(connectionString);
            });

            // Register Category Service
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<SQLServerCategoryRepository>();

            // Register Product Service
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<SQLServerProductRepository>();

            // Register Order Service
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<SQLServerOrderRepository>();
            

            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen(option => {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { In = ParameterLocation.Header, Description = "Please enter a valid token", Name = "Authorization", Type = SecuritySchemeType.Http, BearerFormat = "JWT", Scheme = "Bearer" });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                        new string[] { }
                    }
                });
            });

            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
