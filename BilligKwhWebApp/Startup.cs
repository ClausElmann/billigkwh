using BilligKwhWebApp.Middleware;
using BilligKwhWebApp.Tools.Auth;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using BilligKwhWebApp.Core.Domain;
using FluentValidation.AspNetCore;
using BilligKwhWebApp.Core.Toolbox.JsonConverters;
using BilligKwhWebApp.Core;
using BilligKwhWebApp.Services;
using BilligKwhWebApp.Core.Caching;
using BilligKwhWebApp.Core.Caching.Interfaces;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Services.Localization;
using BilligKwhWebApp.Infrastructure.RootFolder;
using MediatR;
using BilligKwhWebApp.Core.Factories;
using BilligKwhWebApp.Tools.Url;
using System.Net.Http;
using BilligKwhWebApp.Services.Customers.Repository;
using BilligKwhWebApp.Services.Customers;
using BilligKwhWebApp.Services.Dokuments.Repository;
using BilligKwhWebApp.Services.Arduino.Repository;
using BilligKwhWebApp.Services.Arduino;
using BilligKwhWebApp.Services.Electricity.Repository;
using BilligKwhWebApp.Services.Electricity;

namespace BilligKwhWebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; set; }

        private readonly ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            Environment = environment;
            _loggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EnvironmentSettings>(Configuration.GetSection("EnvironmentSettings"));

            services
                .AddMvc(options => { options.EnableEndpointRouting = false; })
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                });

            //InitValidators(services);

            // Add framework services.
            services.AddControllers()
                              .AddJsonOptions(options =>
                              {
                                  options.JsonSerializerOptions.WriteIndented = true;
                                  options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                                  options.JsonSerializerOptions.AllowTrailingCommas = true;
                                  options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                                  options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
                                  options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                                  options.JsonSerializerOptions.Converters.Add(new Core.Toolbox.JsonConverters.DateTimeConverter());
                                  options.JsonSerializerOptions.Converters.Add(new Core.Toolbox.JsonConverters.StringConverter());
                                  options.JsonSerializerOptions.Converters.Add(new IntConverter());
                                  options.JsonSerializerOptions.Converters.Add(new Core.Toolbox.JsonConverters.DoubleConverter());
                                  options.JsonSerializerOptions.Converters.Add(new LongConverter());
                                  options.JsonSerializerOptions.Converters.Add(new JsonTimeSpanConverter());
                              });


            services.AddHttpClient();

            services.AddMediatR(typeof(IBaseRepository).Assembly);

            // Extend View (.cshtml) Locations
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

            // Localization and Options??
            services.AddLocalization();
            services.AddOptions();

            // Authentication 'Jwt Options'
            services
                .AddAuthentication(o =>
                {
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultSignInScheme = "External";
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.TokenValidationParameters.IssuerSigningKey = TokenAuthOption.Key;
                    jwtBearerOptions.TokenValidationParameters.ValidAudience = TokenAuthOption.Audience;
                    jwtBearerOptions.TokenValidationParameters.ValidIssuer = TokenAuthOption.Issuer;

                    // When receiving a token, check that we've signed it.
                    jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;

                    // When receiving a token, check that it is still valid.
                    jwtBearerOptions.TokenValidationParameters.ValidateLifetime = true;
                    jwtBearerOptions.TokenValidationParameters.RequireExpirationTime = true;

                    // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time
                    // when validating the lifetime. As we're creating the tokens locally and validating them on the same
                    // machines which should have synchronised time, this can be set to zero. Where external tokens are
                    // used, some leeway here could be useful.

                    jwtBearerOptions.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                })
                .AddCookie("Application")
                .AddCookie("External");

            // Authorization 'Policies'
            services.AddAuthorization(options =>
            {
                // logged in?
                options
            .AddPolicy(UserRolePermissionProvider.Bearer, new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build());


                // Super Admin
                options
            .AddPolicy(UserRolePermissionProvider.SuperAdmin, policy => policy.AddRequirements(
            new SuperAdminRequirementV2()));
            });

            // Cache And Compress
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            InitServices(services, Environment);

            if (Environment.IsDevelopment())
            {
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "BilligKwh API Documentation",
                        Version = "v1",
                    });
                    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                });
            }

            // Configure 'Cross Origin Resource Sharing'
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });

            // Environment & ConnectionString
            var connectionString = "";
            if (Environment.IsDevelopment() || Environment.IsStaging())
            {
                connectionString = Configuration.GetConnectionString("farmgain_com_db_feederDev");
            }
            else
            {
                connectionString = Configuration.GetConnectionString("farmgain_com_db_feeder");
            }

            // Setup DapperPlus general settings
            DapperPlusRegistration.SetupDapperPlus("Startup BilligKwhWebApp");

            // ConnectionFactory
            ConnectionFactory.ConnectionString = connectionString;

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IOptions<EnvironmentSettings> environmentSettings)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(options =>
                {
                    options.Run(
                        context =>
                        {
                            var ex = context.Features.Get<IExceptionHandlerFeature>();
                            if (ex != null)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            }

                            return Task.CompletedTask;
                        });
                });

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Authorization & Authentication
            app.UseAuthentication();
            app.UseRequestLocalization();

            app.UseWhen(context => !context.Request.Path.StartsWithSegments("/Api/Arduino", StringComparison.OrdinalIgnoreCase),
                           builder => builder.UseHttpsRedirection());

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                      Path.Combine(Directory.GetCurrentDirectory(), "Static_Files")),
                RequestPath = new PathString("/StaticFiles") // requests must then go to /StaticFiles/ and not /Static_files/
            });

            if (Environment.IsDevelopment())
            {
                // Api Docs
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    //c.RoutePrefix = "docs";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BilligKwh API");
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseWhen(context => RequestResponseLoggingMiddlewareFilter.RequestResponseLoggingMiddlewareRules(context), appBuilder =>
            {
                //Add our logging middleware to the pipeline
                appBuilder.UseMiddleware<RequestResponseLoggingMiddleware>();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                if (Environment.IsDevelopment())
                {
                    endpoints.MapControllerRoute(
                       name: "swagger",
                       pattern: "{controller}/{action}/{id?}");
                }
            });

            app.MapWhen(context => context.Request.Host.Host != environmentSettings?.Value?.SubscriptionAppHostName, config =>
            {
                if (!Environment.IsDevelopment())
                {
                    config.UseSpaStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "BilligKwhApp/dist"))
                    });
                }

                config.UseSpa(spa =>
                {
                    // To learn more about options for serving an Angular SPA from ASP.NET Core,
                    // see https://go.microsoft.com/fwlink/?linkid=864501

                    spa.Options.SourcePath = "BilligKwhApp";

                    if (Environment.IsDevelopment())
                    {
                        //spa.UseAngularCliServer(npmScript: "start");
                        spa.UseProxyToSpaDevelopmentServer("http://localhost:4202");
                    }

                    spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "BilligKwhApp/dist"))
                    };
                });
            });
        }

        private static void InitServices(IServiceCollection services, IWebHostEnvironment Environment)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUserUrlGenerator, UserUrlGenerator>();

            services.AddScoped<IAuthorizationHandler, SuperAdminHandlerV2>();

            //cache managers
            services.AddScoped<ICacheManager, PerRequestCacheManager>();
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();

            services.AddScoped<IWorkContext, WebWorkContext>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IGloballyUniqueIdentifier, GloballyUniqueIdentifier>();
            services.AddScoped<IUtcClock, SystemUtcClock>();
            services.AddScoped<ISystemLogger, DefaultLogger>();
            services.AddScoped<ILogService, LogService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IBaseRepository, BaseRepository>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ILanguageService, LanguageService>();

            services.AddScoped<ILocalizationService, LocalizationService>();

            services.AddScoped<IErrorEmailSender, ErrorEmailSender>();

            services.AddScoped<IWebHelper, WebHelper>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IApplicationSettingService, ApplicationSettingService>();
            services.AddScoped<IRootFolderService, RootFolderService>();
            services.AddScoped<IIconService, IconService>();

            services.AddScoped<ISettingsService, SettingsService>();

            services.AddScoped<IDokumentsRepository, DokumentsRepository>();
            services.AddScoped<IDokumentService, DokumentService>();

            services.AddScoped<IArduinoRepository, ArduinoRepository>();
            services.AddScoped<IArduinoService, ArduinoService>();

            services.AddScoped<IElectricityRepository, ElectricityRepository>();
            services.AddScoped<IElectricityService, ElectricityService>();

            //Factories
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<ICustomerFactory, CustomerFactory>();

            // Services
            if (Environment.IsDevelopment())
            {
                services.AddHostedService<LocalDebugService>();
            }
        }
    }
}
