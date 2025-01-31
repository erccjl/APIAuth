﻿using APIAUTH.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace APIAUTH.Data.Context
{
    public class AuthContext : DbContext
    {

        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
        }

        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<CollaboratorType> CollaboratorTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collaborator>()
                       .Navigation(e => e.Role)
                       .AutoInclude();
            modelBuilder.Entity<Collaborator>()
                       .Navigation(e => e.Organization)
                       .AutoInclude();
            modelBuilder.Entity<Collaborator>()
                       .Navigation(e => e.User)
                       .AutoInclude();
            base.OnModelCreating(modelBuilder);
        }
    }
}
