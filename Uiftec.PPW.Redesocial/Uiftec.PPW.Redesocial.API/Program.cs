using Uniftec.PPW.Redesocial.FeedAPI.Dominio.Repositorio;
using Uniftec.PPW.Redesocial.FeedAPI.Repositorio;

namespace Uiftec.PPW.Redesocial.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // 🔧 Registro da dependência para injeção de dependência
            builder.Services.AddScoped<IFeedRepositorio, FeedRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}