using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Share.Infrastructure.UnitOfWork.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Data.EntityFramework.Context
{
    public class ClubContext : EntityFrameworkContext
    {
        private IConfiguration Configuration { get; set; }

        public ClubContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            this.Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(this.Configuration["ConnectionString"], builder =>
            {
                builder.UseRowNumberForPaging();
                builder.MigrationsAssembly("DotNetBluc.Web");
            });
        }
    }
}
