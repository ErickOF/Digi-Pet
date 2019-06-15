using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Helpers;
using WebApi.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using System;
using Microsoft.AspNetCore.HttpOverrides;
using WebApi.Entities;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebApi.Model;
using WebApi.Dtos;

namespace WebApi
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
            services.AddCors();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

    

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRepository,Repository>();
            services.AddScoped<IConfigurationService, ConfigurationService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("ApplicationDbContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IUserService userService,IConfigurationService configService,
            IOptions<AppSettings> appSettingsOpts, IRepository repository)
        {

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            
            app.UseMvc();
            var testOwnerConfig = Configuration.GetSection("TestOwner");
            var ownerDto = testOwnerConfig.Get<OwnerDto>();

            var adminFromConfig = Configuration.GetSection("Admin");
            var admin = adminFromConfig.Get<User>();
           
            try
            {
                //se agrega usuario por defecto
                if (!userService.UsernameExists(admin.Username))
                {
                    var user = userService.CreateUser(admin, Configuration["Admin:Password"]);
                    userService.PersistUser(user).Wait();
                }
                if (!userService.UsernameExists(ownerDto.Email))
                {
                    repository.CreateOwner(ownerDto).Wait();
                }
                

            }
            catch { }

            try
            {
                configService.AddConfig(appSettingsOpts.Value).Wait();
            }
            catch
            {

            }
        
            
        }
    }
}
