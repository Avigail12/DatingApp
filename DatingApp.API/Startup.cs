using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
             services.AddDbContext<DataContext>(x => {
                 x.UseLazyLoadingProxies();
                 x.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
             });

             ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
             services.AddDbContext<DataContext>(x => {
                 x.UseLazyLoadingProxies();
                 x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
             });

             ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(opt => {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddCors();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddAutoMapper(typeof(DatingRepository).Assembly);
            //use this instance with few reqest in the same time
            services.AddScoped<IAuthRepository,AuthRepository>();
            services.AddScoped<IDatingRepository, DatingRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(Options =>{             
                    Options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                        .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer=false,
                        ValidateAudience=false

                    };

                });
            services.AddScoped<LogUserActivity>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            { 
                //כדי לא לעשות את כל הפרוייקט עם TRY ן-CATCH קיצור דרך שיהיה לכל הפרוייקט EXCEPTIONHANDALER
                app.UseExceptionHandler(builder => {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                   
                        //this error is going to store our particular error
                        var error=context.Features.Get<IExceptionHandlerFeature>();
                        if(error != null)
                        {
                            context.Response.AddAplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
                // app.UseHttpsRedirection();
                app.UseHsts();
            }
           

            app.UseRouting();


            app.UseCors(x =>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();

            app.UseAuthorization();

            // add the support for serving static files. --publish

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "FallBack");
            });
        }
    }
}
//Integrated Security=true  User Id=appuser; Password=password
// "ConnectionStrings":{
//   "DefaultConnection":"Server=(local)\\sqlexpress; Database=datingapp; User Id=appuser; Password=Avigail12"
// },