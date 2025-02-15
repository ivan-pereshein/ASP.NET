using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess
{
    public class EfContext: DbContext
    {
        public EfContext(DbContextOptions<EfContext> options) : base(options)
        {
        }

        // Employee, Roles, Customer, Preference и PromoCode
        public DbSet<Employee> Employees { get; set; }
                
        public DbSet<Role> Roles { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Preference> Preferences { get; set; }

        public DbSet<PromoCode> PromoCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*

            Настроить маппинг классов Employee, Roles, Customer,Preference и PromoCode на базу данных через EF. 
            PromoCode имеет ссылку на Preference и Employee. 
            Employee имеет ссылку на Role. 
            Customer имеет набор Preference, но так как Preference - это общий справочник и сущности связаны 
            через Many-to-many, то нужно сделать маппинг через сущность CustomerPreference. 

            Связь Customer и Promocode реализовать через One-To-Many, 
            будем считать, что в данном примере промокод может быть выдан только одному клиенту из базы.

            Строковые поля должны иметь ограничения на MaxLength. 

             */

            // PromoCode имеет ссылку на Preference
            modelBuilder.Entity<PromoCode>()
                .HasOne<Preference>(code => code.Preference)
                .WithMany()
                .IsRequired();

            // PromoCode имеет ссылку на Employee
            modelBuilder.Entity<PromoCode>()
                .HasOne<Employee>(code => code.PartnerManager)
                .WithMany()
                .IsRequired();

            // Customer многие ко многим с Preference
            modelBuilder.Entity<CustomerPreference>()
                .HasOne<Customer>(m => m.Customer)
                .WithMany(c => c.CustomerPreferences)
                .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<CustomerPreference>()
                .HasOne<Preference>(m => m.Preference)
                .WithMany()
                .HasForeignKey(c => c.PreferenceId);

            // Customer has many promocodes. Promocode can belong to only one customer.
            modelBuilder.Entity<Customer>()
                .HasMany<PromoCode>(c => c.PromoCodes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            // Employee имеет ссылку на Role
            modelBuilder.Entity<Employee>()
                .HasOne<Role>(e => e.Role)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Customer>().Property(c => c.FirstName).HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.LastName).HasMaxLength(100);
            modelBuilder.Entity<Customer>().Property(c => c.Email).HasMaxLength(100);

            modelBuilder.Entity<Preference>().Property(c => c.Name).HasMaxLength(100);

            modelBuilder.Entity<PromoCode>().Property(c => c.ServiceInfo).HasMaxLength(100);
            modelBuilder.Entity<PromoCode>().Property(c => c.ServiceInfo).HasMaxLength(100);

            modelBuilder.Entity<CustomerPreference>().HasKey(nameof(CustomerPreference.CustomerId), nameof(CustomerPreference.PreferenceId));

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
