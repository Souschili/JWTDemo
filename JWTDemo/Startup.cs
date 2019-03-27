using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWTDemo
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
            // по хорошему это надо вынести либо в статик класс ,либо в аппсетинг 
            #region
            // security key
            var security_key = "this_is_very_powerfull_secret_key_2019";

            // symetrick security key
            var symetrickSecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(security_key));

            // singing credentials
            var singingcredentials = new SigningCredentials(symetrickSecKey, SecurityAlgorithms.HmacSha256);
            #endregion

            //добавим валидацию токена
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer( options=> {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // что будем валидировать
                        ValidateIssuer=true,
                        ValidateAudience=true,
                        ValidateIssuerSigningKey=true,
                        // правильные парметры,с чем сравнивать
                        ValidAudience= "http://www.evil.org",
                        ValidIssuer= "http://www.evil.org",
                        IssuerSigningKey=symetrickSecKey,



                    };

                });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
