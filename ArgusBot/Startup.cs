using ArgusBot.BLL.Services.Implementation;
using ArgusBot.BLL.Services.Interfaces;
using ArgusBot.DAL.Repositories.Implementation;
using ArgusBot.DAL.Repositories.Interfaces;
using ArgusBot.Data;
using ArgusBot.Services;
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

            services.AddDbContext<UsersContext>(options => options.UseSqlServer(connection));

            services.AddHttpContextAccessor();

            services.AddControllersWithViews();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();


            services.AddHttpClient("tgclient").AddTypedClient<ITelegramBotClient>(client => new TelegramBotClient(Configuration["bot-token"]));
            services.AddScoped<HandleUpdateService>();

            services.AddHostedService<TelegramHostedService>().AddLogging(log => log.AddConsole());

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
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
