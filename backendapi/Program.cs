
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace backendapi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {   // This authority is the tenant ID
            //        options.Authority = "https://login.microsoftonline.com/7ed5147d-c6ee-4886-9e71-4a91ce3087ba";
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = false, // You might want to validate this
            //            ValidAudience = "fa0f1aa9-eea3-4802-9229-83e4c60c64ea" // The client id
            //        };
            //    });

            builder.Services.AddTransient<BlobStorageService>();


           //Add services to the container.
           builder.Services.AddCors(options =>
           {
               options.AddPolicy(name: MyAllowSpecificOrigins,
                                 builder =>
                                 {
                                     builder.WithOrigins("https://agreeable-meadow-0c593c303.3.azurestaticapps.net/",
                                                         "http://localhost:4200",
                                                         "localhost:4200",
                                                         "https://apibackendmartyannick.azurewebsites.net/api/v1/users")
                                                           .AllowAnyMethod()
                                                           .AllowAnyHeader();
                                 });
           });





            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            app.Run();

        }
    }
}