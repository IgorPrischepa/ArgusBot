using ArgusBot.BL.DTO.Mapper;
using ArgusBot.BL.Services.Implementation;
using ArgusBot.BL.Services.Interfaces;
using ArgusBot.DAL.Repositories.Implementation;
using ArgusBot.DAL.Repositories.Interfaces;
using ArgusBot.Data;
using ArgusBot.Services.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace ArgusBot
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
            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<MainContext>(options => options.UseSqlServer(connection));

            services.AddHttpContextAccessor();

            services.AddControllersWithViews().AddNewtonsoftJson();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();


            services.AddHttpClient("tgclient").AddTypedClient<ITelegramBotClient>(client => new TelegramBotClient(Configuration["bot-token"]));
            services.AddScoped<IHandleUpdateService, HandleUpdateService>();

            services.AddHostedService<TelegramHostedService>().AddLogging(log => log.AddConsole());
            services.AddAutoMapper(typeof(UserMapper));
            services.AddScoped<IUserService, UserService>().AddLogging(log => log.AddConsole());
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ICheckListRepository, CheckListRepository>();
            services.AddScoped<ICheckListServiceInterface, CheckService>();

            services.AddScoped<ISignInService, SignInService>().AddLogging(log => log.AddConsole());
            services.AddTransient<IQueryParser, QueryParser>();
            services.AddScoped<ISignInService, SignInService>();

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                var token = Configuration["bot-token"];
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(name: "tgwebhook",
                                        pattern: $"bot/{token}",
                                        new { controller = "Webhook", action = "Post" });
            });
        }
    }
}
