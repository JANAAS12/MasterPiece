using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MasterPiece.Models;

public partial class PetNet2Context : DbContext
{
    public PetNet2Context()
    {
    }

    public PetNet2Context(DbContextOptions<PetNet2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Clinic> Clinics { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PetOwner> PetOwners { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceType> ServiceTypes { get; set; }

    public virtual DbSet<SubscriptionPackage> SubscriptionPackages { get; set; }

    public virtual DbSet<TransportRequest> TransportRequests { get; set; }

    public virtual DbSet<Vet> Vets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-JP544BA;Database=PetNet2;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admins__3213E83F9CC34DDE");

            entity.HasIndex(e => e.Email, "UQ__Admins__AB6E6164E0CCA931").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Appointm__3213E83F88D4383E");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppointmentDate)
                .HasColumnType("datetime")
                .HasColumnName("appointment_date");
            entity.Property(e => e.AppointmentTime)
                .HasColumnType("datetime")
                .HasColumnName("appointment_time");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.ClinicName)
                .HasMaxLength(255)
                .HasColumnName("clinicName");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.OwnerName)
                .HasMaxLength(255)
                .HasColumnName("ownerName");
            entity.Property(e => e.OwnerPhone)
                .HasMaxLength(20)
                .HasColumnName("ownerPhone");
            entity.Property(e => e.PetType)
                .HasMaxLength(255)
                .HasColumnName("petType");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .HasColumnName("serviceName");
            entity.Property(e => e.ServiceTypeId).HasColumnName("service_type_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.Clinic).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ClinicId)
                .HasConstraintName("FK__Appointme__clini__49C3F6B7");

            entity.HasOne(d => d.Owner).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK__Appointme__owner__48CFD27E");

            entity.HasOne(d => d.ServiceType).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ServiceTypeId)
                .HasConstraintName("FK__Appointme__servi__4AB81AF0");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Blog__3213E83F29F8D58B");

            entity.ToTable("Blog");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Likes).HasColumnName("likes");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chat__3213E83F19ED3C4B");

            entity.ToTable("Chat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
        });

        modelBuilder.Entity<Clinic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Clinics__3213E83F4178A42B");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Image1)
                .HasMaxLength(255)
                .HasColumnName("image1");
            entity.Property(e => e.Image2)
                .HasMaxLength(255)
                .HasColumnName("image2");
            entity.Property(e => e.Image3)
                .HasMaxLength(255)
                .HasColumnName("image3");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.WorkingHours)
                .HasMaxLength(255)
                .HasColumnName("working_hours");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3213E83F6BD16EFB");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(255)
                .HasColumnName("payment_method");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(255)
                .HasColumnName("payment_status");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Payments__appoin__5812160E");

            entity.HasOne(d => d.Owner).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Payments__owner___59063A47");
        });

        modelBuilder.Entity<PetOwner>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Pet_Owne__3213E83FC66E7D56");

            entity.ToTable("Pet_Owners");

            entity.HasIndex(e => e.Email, "UQ__Pet_Owne__AB6E616434253B51").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Location)
                .HasMaxLength(20)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Services__3213E83F4A3D254D");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.HasOne(d => d.Clinic).WithMany(p => p.Services)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Services__clinic__4316F928");
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Service___3213E83FC1542DBF");

            entity.ToTable("Service_Types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Img).HasColumnName("img");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceTypes)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Service_T__servi__45F365D3");
        });

        modelBuilder.Entity<SubscriptionPackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3213E83F03010391");

            entity.ToTable("Subscription_Packages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
        });

        modelBuilder.Entity<TransportRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transpor__3213E83F64BFE0DF");

            entity.ToTable("Transport_Requests");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.DropoffAddress)
                .HasMaxLength(255)
                .HasColumnName("dropoff_address");
            entity.Property(e => e.PickupAddress)
                .HasMaxLength(255)
                .HasColumnName("pickup_address");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.Appointment).WithMany(p => p.TransportRequests)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Transport__appoi__5070F446");
        });

        modelBuilder.Entity<Vet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vets__3213E83FE0450765");

            entity.HasIndex(e => e.Email, "UQ__Vets__AB6E61640A1A49F9").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClinicId).HasColumnName("clinic_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.ExperienceYears).HasColumnName("experience_years");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .HasColumnName("image");
            entity.Property(e => e.Location)
                .HasMaxLength(20)
                .HasColumnName("location");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Specialization)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("specialization");

            entity.HasOne(d => d.Clinic).WithMany(p => p.Vets)
                .HasForeignKey(d => d.ClinicId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Vets__clinic_id__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
