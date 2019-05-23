using ExLibrary;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {

      ConfigureLogging();

      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddLogging(); 

      services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("ToDoList"));

      services.AddScoped<IApiService, ApiService>();

      services.AddScoped<ApiServiceWrapper>(); 

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      
      services.AddSwaggerGen(c =>
          c.SwaggerDoc("v1", new Info
          {
            Title = "My Todo Api",
            Version = "v1",
            Description = "A simple example ASP.NET Core Web API",
            TermsOfService = "None",
            Contact = new Contact
            {
              Name = "Shayne Boyer",
              Email = string.Empty,
              Url = "https://twitter.com/spboyer"
            },
            License = new License
            {
              Name = "Use under LICX",
              Url = "https://example.com/license"
            }
          })
      );

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

      
      loggerFactory.AddSerilog();

      app.Use(async (context, next) =>
      {
        context.Items["IsVerified"] = true;
        await next.Invoke();
      });


      app.Run(async (context) =>
      {
        await context.Response.WriteAsync($"Verfied: {context.Items["IsVerified"]}");
      });

      app.UseDefaultFiles();
      app.UseStaticFiles();
      app.UseHttpsRedirection();
      app.UseMvc();

      app.UseSwagger();
      app.UseSwaggerUI(c =>
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Todo Api V1")
      );
    }

    public void ConfigureLogging()
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();
    }

  }
}
