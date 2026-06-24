using Microsoft.EntityFrameworkCore;
using DischargeDisposition_Backend.Insurance.DTOs.Responses;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DischargeDisposition_Backend.Insurance.Models;

namespace DischargeDisposition_Backend.Data
{
    public class InsuranceDbContext : DbContext
    {
        public InsuranceDbContext(
            DbContextOptions<InsuranceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<InsuranceProvider> InsuranceProviders { get; set; } = null!;
        public DbSet<Plan> Plans { get; set; } = null!;
        public DbSet<MemberCoverage> MemberCoverages { get; set; } = null!;
        public DbSet<CoverageRule> CoverageRules { get; set; } = null!;
        public DbSet<AuthorizationRequest> AuthorizationRequests { get; set; } = null!;
        public DbSet<AuthorizationDecision> AuthorizationDecisions { get; set; } = null!;
        public DbSet<InsuranceDashboard>InsuranceDashboard{ get; set; }
        public DbSet<InsuranceAnalytics>InsuranceAnalytics{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var dateOnlyConverter =
                new ValueConverter<DateOnly, DateTime>(
                    d => d.ToDateTime(TimeOnly.MinValue),
                    dt => DateOnly.FromDateTime(dt));

            modelBuilder.Entity<Member>(b =>
            {
                b.Property(m => m.DOB)
                    .HasConversion(dateOnlyConverter)
                    .HasColumnType("date");

                b.Property(m => m.Gender)
                    .HasConversion<string>()
                    .HasMaxLength(10);

                b.HasIndex(m => m.PolicyNumber)
                    .IsUnique();
            });

            modelBuilder.Entity<InsuranceProvider>(b =>
            {
                b.HasIndex(i => i.ProviderCode)
                    .IsUnique();
            });

            modelBuilder.Entity<Plan>(b =>
            {
                b.HasOne(p => p.insuranceProvider)
                    .WithMany(i => i.Plans)
                    .HasForeignKey(p => p.InsuranceProviderId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(p => p.MemberCoverages)
                    .WithOne(mc => mc.plan)
                    .HasForeignKey(mc => mc.PlanId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(p => p.CoverageRules)
                    .WithOne(cr => cr.plan)
                    .HasForeignKey(cr => cr.PlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MemberCoverage>(b =>
            {
                b.HasOne(mc => mc.member)
                    .WithMany(m => m.MemberCoverages)
                    .HasForeignKey(mc => mc.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(mc => mc.plan)
                    .WithMany(p => p.MemberCoverages)
                    .HasForeignKey(mc => mc.PlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CoverageRule>(b =>
            {
                b.HasOne(cr => cr.plan)
                    .WithMany(p => p.CoverageRules)
                    .HasForeignKey(cr => cr.PlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AuthorizationRequest>(b =>
            {
                b.Property(ar => ar.Status)
                    .HasConversion<string>()
                    .HasMaxLength(30);

                b.Property(ar => ar.RequestedDate)
                    .HasColumnType("datetime2");

                b.HasOne(ar => ar.member)
                    .WithMany(m => m.AuthorizationRequests)
                    .HasForeignKey(ar => ar.MemberId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AuthorizationDecision>(b =>
            {
                b.Property(ad => ad.DecisionStatus)
                    .HasConversion<string>()
                    .HasMaxLength(30);

                b.Property(ad => ad.DecisionDate)
                    .HasColumnType("datetime2");

                b.HasOne(ad => ad.authorizationRequest)
                    .WithMany(ar => ar.AuthorizationDecisions)
                    .HasForeignKey(ad => ad.AuthorizationRequestId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<InsuranceDashboard>()
                .HasNoKey()
                .ToView("vwInsuranceDashboard");
            modelBuilder
                .Entity<InsuranceAnalytics>()
                .HasNoKey()
                .ToView("vwInsuranceServiceAnalytics");

        }
    }
}