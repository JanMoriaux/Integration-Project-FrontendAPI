using FrontEndAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEndAPI.Models.Database
{
    public class S2ITSP2_2_Context : DbContext
    {
        public S2ITSP2_2_Context(DbContextOptions<S2ITSP2_2_Context> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //ignore the roles enum
            modelBuilder.Entity<User>()
            .Ignore(u => u.Roles);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("api_users");

                //indexes
                entity.HasIndex(e => new { e.UUID, e.IsActive }).IsUnique();
                entity.HasIndex(e => new { e.Email, e.IsActive }).IsUnique();

                //columns
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UUID).HasColumnName("uuid").IsRequired().HasMaxLength(36);
                entity.Property(e => e.Version).HasColumnName("version").IsRequired();
                entity.Property(e => e.IsActive).HasColumnName("isactive").IsRequired();
                entity.Property(e => e.LastUpdated).HasColumnName("lastupdated").HasColumnType("timestamp").IsRowVersion();
                entity.Property(e => e.Firstname).HasColumnName("firstname").IsRequired().HasMaxLength(500);
                entity.Property(e => e.Lastname).HasColumnName("lastname").IsRequired().HasMaxLength(500);
                entity.Property(e => e.Email).HasColumnName("email").IsRequired().HasMaxLength(500);
                entity.Property(e => e.Password).HasColumnName("password").IsRequired().HasMaxLength(500);
                entity.Property(e => e.Salt).HasColumnName("salt").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Street).HasColumnName("street").IsRequired().HasMaxLength(500);
                entity.Property(e => e.Number).HasColumnName("number").IsRequired().HasMaxLength(500);
                entity.Property(e => e.Bus).HasColumnName("bus").HasMaxLength(24);
                entity.Property(e => e.ZipCode).HasColumnName("zipcode").IsRequired();
                entity.Property(e => e.City).HasColumnName("city").IsRequired().HasMaxLength(500);
                entity.Property(e => e.RolesString).HasColumnName("roles").IsRequired().HasMaxLength(30);
                entity.Property(e => e.EmailVerified).HasColumnName("emailverified").IsRequired();
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("api_events");

                //indexes
                entity.HasIndex(e => new { e.UUID, e.IsActive }).IsUnique();
                entity.HasIndex(e => new { e.Name, e.IsActive }).IsUnique();

                //columns
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UUID).HasColumnName("uuid").IsRequired().HasMaxLength(36);
                entity.Property(e => e.Version).HasColumnName("version").IsRequired();
                entity.Property(e => e.IsActive).HasColumnName("isactive").IsRequired();
                entity.Property(e => e.LastUpdated).HasColumnName("lastupdated").HasColumnType("timestamp").IsRowVersion();
                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(500);
                entity.Property(e => e.Description).HasColumnName("description").IsRequired().HasColumnType("text");
                entity.Property(e => e.StartTime).HasColumnName("starttime").IsRequired();
                entity.Property(e => e.EndTime).HasColumnName("endtime").IsRequired();
                entity.Property(e => e.ImageURL).HasColumnName("imageurl").HasMaxLength(500);
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("api_activities");

                //indexes
                entity.HasIndex(e => new { e.UUID, e.IsActive }).IsUnique();
                entity.HasIndex(e => new { e.Name, e.UUID, e.IsActive }).IsUnique();

                //columns
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UUID).HasColumnName("uuid").IsRequired().HasMaxLength(36);
                entity.Property(e => e.Version).HasColumnName("version").IsRequired();
                entity.Property(e => e.IsActive).HasColumnName("isactive").IsRequired();
                entity.Property(e => e.LastUpdated).HasColumnName("lastupdated").HasColumnType("timestamp").IsRowVersion();
                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(500);
                entity.Property(e => e.Description).HasColumnName("description").IsRequired().HasColumnType("text");
                entity.Property(e => e.StartTime).HasColumnName("starttime").IsRequired();
                entity.Property(e => e.EndTime).HasColumnName("endtime").IsRequired();
                entity.Property(e => e.EventId).HasColumnName("eventid").IsRequired();
                entity.Property(e => e.SpeakerId).HasColumnName("speakerid");
                entity.Property(e => e.Price).HasColumnName("price").IsRequired();
                entity.Property(e => e.RemainingCapacity).HasColumnName("remainingcapacity").IsRequired();

                //foreign key constraints
                entity.HasOne(d => d.Event);
                entity.HasOne(d => d.Speaker);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("api_reservations");

                //indexes
                entity.HasIndex(e => new { e.UUID, e.IsActive }).IsUnique();
                entity.HasIndex(e => new { e.ActivityId, e.VisitorId, e.IsActive }).IsUnique();

                //columns
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UUID).HasColumnName("uuid").IsRequired().HasMaxLength(36);
                entity.Property(e => e.Version).HasColumnName("version").IsRequired();
                entity.Property(e => e.IsActive).HasColumnName("isactive").IsRequired();
                entity.Property(e => e.LastUpdated).HasColumnName("lastupdated").HasColumnType("timestamp").IsRowVersion();
                entity.Property(e => e.ActivityId).HasColumnName("activityid").IsRequired();
                entity.Property(e => e.VisitorId).HasColumnName("visitorid").IsRequired();
                entity.Property(e => e.PayedFee).HasColumnName("payedfee").IsRequired();
                entity.Property(e => e.HasAttended).HasColumnName("hasattended").IsRequired();
                entity.Property(e => e.WithInvoice).HasColumnName("withinvoice").IsRequired();

                //foreign key constraints
                entity.HasOne(r => r.Activity);
                entity.HasOne(r => r.Visitor);
            });
        }
    }
}
