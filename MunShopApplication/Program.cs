using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MunShopApplication.Repository;
using MunShopApplication.Repository.SQLServer;
using MunShopApplication.Services;
using MunShopApplication.Configs;
using System.Text;
using System.Security.Claims;
using MunShopApplication.Commons;

namespace MunShopApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //bind object model from configuration
            JWTConfig jwtConfig = builder.Configuration.GetSection("jwt").Get<JWTConfig>();
            //add it to services
            builder.Services.AddSingleton(jwtConfig);

            builder.Services.AddScoped<SqlConnection>(sp =>
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

            // Register User Service
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<SQLServerUserRepository>();

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

            //JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", p => p.RequireClaim(ClaimTypes.Role, ((int) RoleEnum.Admin).ToString()));
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
