using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortalVioo.Models;
using PortalVioo.ModelsApp;

namespace PortalVioo.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.ApplyConfiguration(new Role());
        }

        public DbSet<ParamType> ParamType { get; set; }
        public DbSet<ParamStatus> ParamStatus { get; set; }
        public DbSet<ParamPriorite> ParamPriorite { get; set; }

        public DbSet<ApplicationUser> AspNetUsers;
        public DbSet<Projet> Projet { get; set; }
        public DbSet<MembreProjet> MembreProjet { get; set; }



    }
}
