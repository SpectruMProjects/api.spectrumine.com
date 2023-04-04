
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SpectruMineAPI.Services.Auth;
using SpectruMineAPI.Services.Database;
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
            builder.Services.Configure<DBSettings>(builder.Configuration.GetSection("MongoSettings"));
            builder.Services.Configure<MailData>(builder.Configuration.GetSection("SMTPData"));
            AuthOptions.KEY = builder.Configuration.GetValue<string>("JWTSecret")!;
            //Db services
            builder.Services.AddSingleton<MongoService>();
            builder.Services.AddSingleton<UserCRUD>();
            builder.Services.AddSingleton<HCStatsCRUD>();
            //
            builder.Services.AddSingleton<MailSenderService>();
            builder.Services.AddSingleton<MailService>();
            builder.Services.AddSingleton<AuthService>();
            builder.Services.AddSingleton<HardcoreService>();
            var app = builder.Build();

            app.UseCors(p => p
              .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();
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
