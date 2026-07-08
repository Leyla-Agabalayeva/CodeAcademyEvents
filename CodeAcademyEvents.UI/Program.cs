using AutoMapper;
using CodeAcademyEvents.BLL.Mapper;
using CodeAcademyEvents.BLL.Services.Implementations;
using CodeAcademyEvents.BLL.Services.Interfaces;
using CodeAcademyEvents.DAL.Data;
using CodeAcademyEvents.DAL.Repositories;
using CodeAcademyEvents.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeAcademyEvents.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            var builder = WebApplication.CreateBuilder(args);

            // ---------- DbContext (AppDbContext artıq IdentityDbContext-dən miras alır) ----------
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ---------- Identity (Login/Register/Rollar: Admin) ----------
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            builder.Services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, IdentityEmailSender>();

            // ---------- AutoMapper ----------
            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
            // ---------- Repository / UnitOfWork ----------
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ---------- Servislər ----------
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<IInvitationService, InvitationService>();
            builder.Services.AddScoped<IParticipationService, ParticipationService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<IStatisticsService, StatisticsService>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // ================= VACİB: Admin hesabını avtomatik yaratmaq =================
            // Bu, tətbiq hər dəfə başlayanda "Admin" rolunu və 1 default admin istifadəçisini yaradır
            // (əgər onsuz da mövcud deyilsə). Başqa heç bir data (Event, Location və s.) yaradılmır —
            // onları admin öz hesabı ilə daxil olub sayt üzərindən əlavə edir.
            await DbInitializer.SeedAdminAsync(app.Services);
            // ==============================================================================

            // ---------- Middleware pipeline ----------
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();   // MÜTLƏQ UseAuthorization-dan ƏVVƏL olmalıdır
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages(); // Identity-nin öz Login/Register UI-si üçün

            app.Run();

        }
    }
}
