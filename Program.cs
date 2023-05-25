
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using SpectruMineAPI.Controllers;
using SpectruMineAPI.Services.Auth;
using SpectruMineAPI.Services.Cache;
using SpectruMineAPI.Services.Database;
using SpectruMineAPI.Services.Database.CRUDs;
using SpectruMineAPI.Services.Hardcore;
using SpectruMineAPI.Services.Mail;

namespace SpectruMineAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });
            //Services
            //Db services
            builder.Services.Configure<DBSettings>(builder.Configuration.GetSection("MongoSettings"));
            builder.Services.AddSingleton<MongoService>();
            builder.Services.AddSingleton<UserCRUD>();
            builder.Services.AddSingleton<HCStatsCRUD>();
            builder.Services.AddSingleton<CacheService>();
            //Services
            builder.Services.Configure<MailData>(builder.Configuration.GetSection("SMTPData"));
            builder.Services.AddScoped<MailSenderService>();
            builder.Services.AddScoped<MailService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<HardcoreService>();
            var app = builder.Build();
            app.Services.GetService<CacheService>();
            //CORS
            app.UseCors(p => p
              .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
            //JWT Init
            app.UseAuthentication();
            app.UseAuthorization();
            AuthOptions.KEY = builder.Configuration.GetValue<string>("JWTSecret")!;
            AuthOptions.UseMail = app.Configuration.GetValue<bool>("UseMail");
            AuthOptions.UseMojangChecks = app.Configuration.GetValue<bool>("MojangChecks");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}
