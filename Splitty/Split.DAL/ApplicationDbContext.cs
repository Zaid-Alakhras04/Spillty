using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Split.Core.Entities;

namespace Split.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<ExpenseShare> ExpenseShares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Expenses>()
                .Property(e => e.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ExpenseShare>()
                .Property(es => es.OwnedAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Members)
                .WithOne(m => m.Trip)
                .HasForeignKey(m => m.TripID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Trip>()
                .HasMany(t => t.Expense)
                .WithOne(e => e.Trip)
                .HasForeignKey(e => e.TripID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expenses>()
                .HasOne(e => e.Member)
                .WithMany(m => m.Expense)
                .HasForeignKey(e => e.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExpenseShare>()
                .HasOne(es => es.Expense)
                .WithMany(e => e.Shares)
                .HasForeignKey(es => es.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExpenseShare>()
                .HasOne(es => es.Member)
                .WithMany(m => m.ExpenseShares)
                .HasForeignKey(es => es.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
