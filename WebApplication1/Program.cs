using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

    var startup =new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);
// Add services to the container.



var app = builder.Build();
startup.Configure(app, app.Environment);
// Configure the HTTP request pipeline.


app.Run();
