
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SpectruMineAPI.Services.Auth;
using SpectruMineAPI.Services.Database;

namespace SpectruMineAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = false,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = false,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = false,
                };
            });
            builder.Services.Configure<DBSettings>(builder.Configuration.GetSection("MongoSettings"));
            builder.Services.AddSingleton<UserCRUD>();
            builder.Services.AddSingleton<AuthService>();
            var app = builder.Build();
            app.UseAuthorization();
            app.UseAuthentication();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}