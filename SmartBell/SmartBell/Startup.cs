using Data;
using Logic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Models;
using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SmartBell
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc().AddNewtonsoftJson();
            services.AddDbContext<SbDbContext>();
            services.AddTransient<ModificationLogic, ModificationLogic>();
            services.AddTransient<ReadLogic, ReadLogic>();
            services.AddTransient<TimeLogic, TimeLogic>();
            services.AddTransient<ClientLogic, ClientLogic>();
            services.AddTransient<TemplateEditingLogic, TemplateEditingLogic>();
            services.AddTransient<ITemplateRepository, TemplateRepository>();
            services.AddTransient<ITemplateElementRepository, TemplateEelementRepository>();
            services.AddTransient<IBellRingRepository, BellRingRepository>();
            services.AddTransient<IHolidayRepository, HolidayRepository>();
            services.AddTransient<IOutputPathRepository, OutputPathRepository>();
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "SmartBell Api endpoints", Version = "v1" });
                var filename = $"Models.xml";
                var filepath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).FullName + @"/Models", filename);
                opt.IncludeXmlComments(filepath);
            });

            services.Configure<FormOptions>(opt =>
            {
                opt.ValueCountLimit = int.MaxValue;
                opt.MultipartBodyLengthLimit = int.MaxValue;
                opt.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                                  });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHttpsRedirection();
            app.UseCors();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Output")),
                RequestPath = new PathString("/Output")
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(opt=>
            {
                opt.SwaggerEndpoint("./v1/swagger.json", "Smartbell Api");
            });
        }
    }
}
