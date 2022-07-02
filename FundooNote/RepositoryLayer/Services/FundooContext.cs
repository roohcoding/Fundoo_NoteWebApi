using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Services
{

    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions<FundooContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
    }

}
