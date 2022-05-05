

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using WebApplication1.Filters;
using WebApplication1.Servicios;

namespace WebApplication1
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opstions =>
            {
                opstions.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(options =>
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();

            services.AddDbContext<AplicationDbContext>(options=>
            options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));


            services.AddEndpointsApiExplorer();


            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name ="Authorization",
                    Type =SecuritySchemeType.ApiKey,
                    Scheme ="Bearer",
                    BearerFormat = "JWT",
                    In= ParameterLocation.Header,

                });


                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference= new OpenApiReference
                            {
                                Type= ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opc=>opc.TokenValidationParameters=new TokenValidationParameters
            {
                ValidateIssuer=false,
                ValidateAudience=false,
                ValidateLifetime=true,
                ValidateIssuerSigningKey=true,
                IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["llaveJWT"])),
                ClockSkew=TimeSpan.Zero
                
            });

            //Configurar los servicios
            services.AddTransient<MiFiltrodeAccion>();


            //Configurar automaper
            services.AddAutoMapper(typeof(Startup));


            //configuracion de identity
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AplicationDbContext>().AddDefaultTokenProviders();


            services.AddAuthorization(opc =>
            {
                opc.AddPolicy("EsAdmin", politica => politica.RequireClaim("esAdmin"));
            });


            services.AddCors(opc =>
            {
                opc.AddDefaultPolicy(buld =>
                {
                    buld.WithOrigins("").AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddDataProtection();



            services.AddTransient<HashService>();

        }

        public void Configure(IApplicationBuilder app,IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                
            }
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors();  

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
