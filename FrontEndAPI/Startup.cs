using FrontEndAPI.Configuration;
using FrontEndAPI.Mail;
using FrontEndAPI.Models.Database;
using FrontEndAPI.Models.Database.Repository.ActivityRepo;
using FrontEndAPI.Models.Database.Repository.EventRepo;
using FrontEndAPI.Models.Database.Repository.ReservationRepo;
using FrontEndAPI.Models.Database.Repository.UserRepo;
using FrontEndAPI.Models.Database.Repository.UUIDRepo;
using FrontEndAPI.RabbitMQ.Configuration;
using FrontEndAPI.RabbitMQ.Consumer;
using FrontEndAPI.RabbitMQ.Producer;
using FrontEndAPI.XML;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace FrontEndAPI
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
            services.AddMvc();

            //json serialization
            services.AddMvc().AddJsonOptions(options => {
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            //mail config
            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddTransient<IEmailService, EmailService>();

            //rabbit mq
            services.AddSingleton<IMQConfiguration>(Configuration.GetSection("RabbitMQConfiguration").Get<MQConfiguration>());
            services.AddSingleton<IMQConsumer, MQConsumer>();
            services.AddScoped<IMQProducer, MQProducer>();
            services.AddSingleton<IHostedService, ConsumerService>();

            //system and message configuration
            services.AddSingleton<IMessageTypeConfiguration>(Configuration.GetSection("MessageTypes").Get<MessageTypeConfiguration>());
            services.AddSingleton<ISystemConfiguration>(Configuration.GetSection("Systems").Get<SystemConfiguration>());

            //controllers as service
            services.AddTransient<FrontEndAPI.Controllers.UserController, FrontEndAPI.Controllers.UserController>();
            services.AddTransient<FrontEndAPI.Controllers.ReservationController, FrontEndAPI.Controllers.ReservationController>();

            //xml services
            services.AddScoped<IXMLService, XMLService>(); 

            //db context
            services.AddDbContext<S2ITSP2_2_Context>(options => options.UseMySQL(
                Configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsHistoryTable("__APIMigrationsHistory")));
            services.AddScoped<IUserRepository, UserRepositoryImpl>();
            services.AddScoped<IEventRepository, EventRepositoryImpl>();
            services.AddScoped<IActivityRepository, ActivityRepositoryImpl>();
            services.AddScoped<IReservationRepository, ReservationRepositoryImpl>();
            services.AddScoped<IUUIDRepository,UUIDRepositoryImpl>();

            //swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Integration Project Frontend API",
                    Description = "API for connecting Wordpress Frontend to Rabbit MQ",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "Jan Moriaux", Email = "jan.moriaux@student.ehb.be", Url = "" }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Frontend API V1");
            });
        }
    }    
}
