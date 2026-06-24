using DischargeDisposition_Backend.Hospital.Models;
using DischargeDisposition_Backend.Hospital.DTOs.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DischargeDisposition_Backend.Data
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuthorizationTracking> AuthorizationTrackings { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<DispositionDecision> DispositionDecisions { get; set; } = null!;
        public DbSet<DelayReasonCode> DelayReasonCodes { get; set; } = null!;
        public DbSet<Payer> Payers { get; set; } = null!;
        public DbSet<DispositionType> DispositionTypes { get; set; } = null!;
        public DbSet<PostAcuteProvider> PostAcuteProviders { get; set; } = null!;
        public DbSet<Referral> Referrals { get; set; } = null!;
        public DbSet<PatientDelay> PatientDelays { get; set; } = null!;
        public DbSet<LengthOfStayTracking> LengthOfStayTrackings { get; set; } = null!;
        public DbSet<HospitalDashboard>HospitalDashboard{ get; set; }
        public DbSet<PatientDistribution>PatientDistribution{ get; set; }

        public DbSet<AuthorizationAnalytics> AuthorizationAnalytics{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var dateOnlyConverter =
                new ValueConverter<DateOnly, DateTime>(
                    d => d.ToDateTime(TimeOnly.MinValue),
                    dt => DateOnly.FromDateTime(dt));

            modelBuilder.Entity<Patient>(b =>
            {
                b.Property(p => p.Dob)
                    .HasConversion(dateOnlyConverter)
                    .HasColumnType("date");

                b.Property(p => p.ExpectedDischargeDate)
                    .HasConversion(dateOnlyConverter)
                    .HasColumnType("date");

                b.Property(p => p.AdmissionDate)
                    .HasColumnType("datetime2");

                b.Property(p => p.ActualDischargeDate)
                    .HasColumnType("datetime2");

                b.Property(p => p.Gender)
                    .HasConversion<string>()
                    .HasMaxLength(10);

                b.HasIndex(p => p.Mrn)
                    .IsUnique();
            });

            modelBuilder.Entity<User>(b =>
            {
                b.Property(u => u.CreatedAt)
                    .HasColumnType("datetime2");
            });

            modelBuilder.Entity<DispositionDecision>(b =>
            {
                b.Property(d => d.DecisionDate)
                    .HasColumnType("datetime2");

                b.Property(d => d.ExpectedTransitionDate)
                    .HasConversion(dateOnlyConverter)
                    .HasColumnType("date");

                b.Property(d => d.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30);

                b.HasOne(d => d.patient)
                    .WithMany(p => p.DispositionDecisions)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(d => d.dispositionType)
                    .WithMany(dt => dt.DispositionDecisions)
                    .HasForeignKey(d => d.DispositionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(d => d.clinician)
                    .WithMany(u => u.DispositionDecisions)
                    .HasForeignKey(d => d.ClinicianId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(d => d.department)
                    .WithMany()
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Referral>(b =>
            {
                b.Property(r => r.CreatedDate)
                    .HasColumnType("datetime2");

                b.Property(r => r.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30);

                b.Property(r => r.Priority)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                b.HasOne(r => r.patient)
                    .WithMany(p => p.Referrals)
                    .HasForeignKey(r => r.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(r => r.provider)
                    .WithMany()
                    .HasForeignKey(r => r.ProviderId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(r => r.careManager)
                    .WithMany(u => u.Referrals)
                    .HasForeignKey(r => r.CareManagerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PostAcuteProvider>(b =>
            {
                b.HasOne(p => p.dispositionType)
                    .WithMany(dt => dt.PostAcuteProviders)
                    .HasForeignKey(p => p.DispositionTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(p => p.user)
                    .WithMany(u => u.PostAcuteProviders)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PatientDelay>(b =>
            {
                b.HasOne(pd => pd.patient)
                    .WithMany(p => p.PatientDelays)
                    .HasForeignKey(pd => pd.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(pd => pd.ReportedUser)
                    .WithMany(u => u.PatientDelays)
                    .HasForeignKey(pd => pd.ReportedBy)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(pd => pd.delayReason)
                    .WithMany()
                    .HasForeignKey(pd => pd.DelayReasonId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LengthOfStayTracking>(b =>
            {
                b.Property(l => l.LastCalculatedDate)
                    .HasColumnType("datetime2");

                b.HasOne(l => l.patient)
                    .WithOne(p => p.lengthOfStayTracking)
                    .HasForeignKey<LengthOfStayTracking>(l => l.PatientId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Payer>(b =>
            {
                b.HasIndex(p => p.PayerName)
                    .IsUnique();
            });

            modelBuilder.Entity<AuthorizationTracking>(b =>
            {
                b.Property(a => a.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30);

                b.Property(a => a.RequestedDate)
                    .HasColumnType("datetime2");

                b.Property(a => a.ResponseDate)
                    .HasColumnType("datetime2");

                b.Property(a => a.LastUpdated)
                    .HasColumnType("datetime2");

                b.HasIndex(a => a.ExternalAuthorizationId)
                    .IsUnique();

                b.HasOne(a => a.patient)
                    .WithMany(p => p.AuthorizationTrackings)
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(a => a.payer)
                    .WithMany(p => p.AuthorizationTrackings)
                    .HasForeignKey(a => a.PayerId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(a => a.referral)
                    .WithOne(r => r.authorizationTrackings)
                    .HasForeignKey<AuthorizationTracking>(a => a.ReferralId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<HospitalDashboard>()
            .HasNoKey()
            .ToView("vwHospitalDashboard");
            modelBuilder
                .Entity<PatientDistribution>()
                .HasNoKey()
                .ToView("vwPatientDistribution");

            modelBuilder
                .Entity<AuthorizationAnalytics>()
                .HasNoKey()
                .ToView("vwAuthorizationAnalytics");
        }
    }
}