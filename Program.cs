﻿using BackEnd_SistemaCompra.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // <-- Asegúrate de tener este using

namespace BackEnd_SistemaCompra
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient();

            builder.Services.AddDbContext<ConexionDB>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql")));

            builder.Services.AddControllers();

            // ✅ Agrega esta configuración explícita
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Sistema de Compras",
                    Version = "v1"
                });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:3000", "https://810-815buyandgo.azurewebsites.net") // ← tu frontend local
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting(); 

            app.UseCors("AllowFrontend");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}