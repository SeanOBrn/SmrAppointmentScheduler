using Microsoft.EntityFrameworkCore;
using SmrAppointmentScheduler.Server.Models;

namespace SmrAppointmentScheduler.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Mechanic> Mechanics => Set<Mechanic>();
    public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
    public DbSet<AppointmentSlot> AppointmentSlots => Set<AppointmentSlot>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<WorkNote> WorkNotes => Set<WorkNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Branch>(b =>
        {
            b.HasMany(x => x.Mechanics).WithOne(x => x.Branch).HasForeignKey(x => x.BranchId);
            b.HasMany(x => x.AppointmentSlots).WithOne(x => x.Branch).HasForeignKey(x => x.BranchId);
        });

        modelBuilder.Entity<Mechanic>(m =>
        {
            m.HasMany(x => x.AppointmentSlots).WithOne(x => x.Mechanic).HasForeignKey(x => x.MechanicId);
        });

        modelBuilder.Entity<ServiceType>(s =>
        {
            s.HasMany(x => x.AppointmentSlots).WithOne(x => x.ServiceType).HasForeignKey(x => x.ServiceTypeId);
        });

        modelBuilder.Entity<AppointmentSlot>(s =>
        {
            s.HasOne(x => x.Appointment).WithOne(x => x.AppointmentSlot).HasForeignKey<Appointment>(a => a.AppointmentSlotId).IsRequired(false);
        });

        modelBuilder.Entity<Appointment>(a =>
        {
            a.HasIndex(x => x.AppointmentSlotId).IsUnique();
            a.HasMany(x => x.WorkNotes).WithOne(x => x.Appointment).HasForeignKey(x => x.AppointmentId);
        });
    }
}
