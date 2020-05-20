using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MetingApi.Data;
using MetingApi.Data.Repositories;
using MetingApi.Models;
using Project.Models;
using Project.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Linq;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace MetingApi
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
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddDbContext<MetingContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MetingContext"))
            );

            services.AddScoped<MetingDataInitializer>();
            services.AddScoped<IMetingRepository, MetingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Register the Swagger services
            services.AddOpenApiDocument(c =>
            {
                c.DocumentName = "OpenAPI 3";
                c.Title = "Meting API";
                c.Version = "v1";
                c.Description = "The Meting API documentation description.";
                c.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme 
                { 
                    Type = OpenApiSecuritySchemeType.ApiKey, 
                    Name = "Authorization", 
                    In = OpenApiSecurityApiKeyLocation.Header, 
                    Description = "Type into the textbox: Bearer {your JWT token}." 
                }); 
                c.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT")); //adds the token when a request is send
            }); //for OpenAPI 3.0 else AddSwaggerDocument();
            //services.AddSwaggerDocument();

            services.AddAuthentication(x => { x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false; x.SaveToken = true; x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = true //Ensure token hasn't expired
                };
            });

            //no UI will be added (<-> AddDefaultIdentity)
                    services.AddIdentity<IdentityUser, IdentityRole>(cfg => cfg.User.RequireUniqueEmail = true).AddEntityFrameworkStores<MetingContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddCors(options => options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin()));          
        }

    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MetingDataInitializer metingDataInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseCors("AllowAllOrigins");

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            metingDataInitializer.InitializeData().Wait();
        }
    }
}
