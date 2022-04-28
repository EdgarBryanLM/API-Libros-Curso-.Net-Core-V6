

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApplication1.Filters;

namespace WebApplication1
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
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
            services.AddSwaggerGen();

            services.AddResponseCaching();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            //Configurar los servicios
            services.AddTransient<MiFiltrodeAccion>();


            //Configurar automaper
            services.AddAutoMapper(typeof(Startup));




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

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
