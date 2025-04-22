﻿using Application.Interfaces;
using BusinessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<UserGroup> UserGroups => Set<UserGroup>();
        public DbSet<GroupPermission> GroupPermissions => Set<GroupPermission>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>().HasKey(ug => new { ug.UserId, ug.GroupId });
            modelBuilder.Entity<GroupPermission>().HasKey(gp => new { gp.GroupId, gp.PermissionId });

            modelBuilder.Entity<UserGroup>()
            .HasOne(ug => ug.User)
            .WithMany(u => u.UserGroups)
            .HasForeignKey(ug => ug.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.GroupId);
            modelBuilder.Entity<UserGroup>()
                .HasKey(ug => new { ug.UserId, ug.GroupId });

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Group)
                .WithMany(g => g.GroupPermissions)
                .HasForeignKey(gp => gp.GroupId);

            modelBuilder.Entity<GroupPermission>()
                .HasOne(gp => gp.Permission)
                .WithMany(p => p.GroupPermissions)
                .HasForeignKey(gp => gp.PermissionId);
        }
        
    }
}
