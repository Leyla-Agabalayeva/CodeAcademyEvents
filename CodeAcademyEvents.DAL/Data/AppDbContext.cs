using CodeAcademyEvents.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Data
{
    // IdentityDbContext-dən miras alır ki, Login/Register/Rollar (Admin, İstifadəçi)
    // eyni DB-də işləsin. IdentityUser -> Person əlaqəsi Person.ApplicationUserId ilə qurulur.
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<EventType> EventTypes { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Organizer> Organizers { get; set; } = null!;
        public DbSet<Invitation> Invitations { get; set; } = null!;
        public DbSet<Participation> Participations { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Event əlaqələri ----------
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Location)
                .WithMany(l => l.Events)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventType)
                .WithMany(t => t.Events)
                .HasForeignKey(e => e.EventTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Organizer)
                .WithMany(o => o.Events)
                .HasForeignKey(e => e.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Invitation əlaqələri ----------
            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Event)
                .WithMany(e => e.Invitations)
                .HasForeignKey(i => i.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Person)
                .WithMany(p => p.Invitations)
                .HasForeignKey(i => i.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            // Eyni şəxs eyni tədbirə iki dəfə dəvət olunmasın
            modelBuilder.Entity<Invitation>()
                .HasIndex(i => new { i.EventId, i.PersonId })
                .IsUnique();

            // ---------- Participation (1-1 Invitation ilə) ----------
            modelBuilder.Entity<Participation>()
                .HasOne(p => p.Invitation)
                .WithOne(i => i.Participation)
                .HasForeignKey<Participation>(p => p.InvitationId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Notification ----------
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Event)
                .WithMany(e => e.Notifications)
                .HasForeignKey(n => n.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Feedback ----------
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Event)
                .WithMany(e => e.Feedbacks)
                .HasForeignKey(f => f.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Person)
                .WithMany(p => p.Feedbacks)
                .HasForeignKey(f => f.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Qeyd: burada artıq HEÇ BİR hazır (fake) biznes data yoxdur.
            // EventType, Location, Organizer, Person, Event — bunların hamısı
            // Admin hesabı ilə daxil olub sayt üzərindən (Admin panel CRUD) əlavə olunur.
            // Yalnız Identity üçün "Admin" rolu və 1 admin istifadəçisi Program.cs-də
            // DbInitializer vasitəsilə avtomatik yaradılır (aşağıya bax).
        }
    }


}
