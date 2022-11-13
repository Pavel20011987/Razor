using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using razor.Models;

namespace razor.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Network> Networks { get; set; }
        public DbSet<Mask> Masks { get; set; }
        public DbSet<NetworkAssignment> NetworkAssignments { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Vlan> Vlans { get; set; }
       
       
       /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Network>().ToTable(nameof(Network));
            modelBuilder.Entity<Mask>().ToTable(nameof(Mask));
            modelBuilder.Entity<Vendor>().ToTable(nameof(Vendor));
            modelBuilder.Entity<Vlan>().ToTable(nameof(Vlan));
            modelBuilder.Entity<NetworkAssignment>().ToTable(nameof(NetworkAssignment));
        }
        */

    }
}
