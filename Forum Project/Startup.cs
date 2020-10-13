using Forum_Project.Context;
using Forum_Project.Models;
using Forum_Project.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace Forum_Project
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
            services.AddMvc(options => 
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddAuthentication()
                .AddGoogle(options => 
                {
                    options.ClientId = "954271961711-ogv6mhggif1ihv4nvrl9vdck5ee8pfpj.apps.googleusercontent.com";
                    options.ClientSecret = "-Ool3bUDobu4wDY_1lwzqKxl";
                }
                );

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Admin") && context.User.HasClaim(claim => claim.Type == "Delete Role" && claim.Value == "true")
                    || context.User.IsInRole("Super Admin")
                    ));

                options.AddPolicy("EditRolePolicy",
                    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                // If one handler returns false then the other handlers will not be checked
                // Useful because handlers that explicitly return false will fail the entire authentication
                // Regardless of the other handlers evaluations
                options.InvokeHandlersAfterFailure = false;

                options.AddPolicy("CreateRolePolicy",
                    policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Admin") && context.User.HasClaim(claim => claim.Type == "Create Role" && claim.Value == "true")
                    || context.User.IsInRole("Super Admin")
                    ));
            });

            services.AddDbContext<ForumDbContext>(item => item.UseSqlServer(Configuration.GetConnectionString("myconn")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<ForumDbContext>().AddDefaultTokenProviders();

            services.AddMailKit(options => options.UseMailKit(Configuration.GetSection("Email").Get<MailKitOptions>()));

            services.AddControllersWithViews();

            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
