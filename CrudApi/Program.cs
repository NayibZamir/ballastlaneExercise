using Crud.Business.Managers;
using Crud.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using CrudApi.Swagger;

namespace CrudApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            SolveDependencies(builder.Services, builder.Configuration);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true

                };
            });

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();



            app.Run();
        }

        public static async  void SolveDependencies(IServiceCollection services, ConfigurationManager configuration)
        {

            string dbConnectionString = configuration.GetConnectionString("dbConnection");
            services.AddTransient<IDbConnection>((sp) => new SqlConnection(dbConnectionString));

            services.AddScoped<IUserRepository, Crud.Data.DataSet>();
            services.AddScoped<IBookRepository, Crud.Data.DataSet>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IBookManager, BookManager>();

            ///TODO; 
            await (new Crud.Data.DataSet(new SqlConnection(dbConnectionString.Replace("Initial Catalog=CrudDb", "Initial Catalog=Master")))).InitializeDatabase();
        }
    }


}