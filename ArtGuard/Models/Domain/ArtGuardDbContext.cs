using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace ArtGuard.Models.Domain
{
    public class ArtGuardDbContext : IdentityDbContext<ApplicationUser>
    {
        public ArtGuardDbContext(DbContextOptions<ArtGuardDbContext> options) : base(options) 
        {

        }

        public DbSet<Cwiczenie> Cwiczenia { get; set; }
        public DbSet<SejsaTreningowa> SesjeTreningowe { get; set; }
        public DbSet<DaneWykonanegoCwiczenia> DaneWykonanychCwiczen { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DaneWykonanegoCwiczenia>().HasOne(w => w.Cwiczenie).WithMany().HasForeignKey(x => x.IdCwiczenia);
            modelBuilder.Entity<DaneWykonanegoCwiczenia>().HasOne(x => x.SesjaTreningowa).WithMany().HasForeignKey(x => x.IdSesjiTreningowej);
            modelBuilder.Entity<SejsaTreningowa>().HasOne(x => x.User).WithMany().HasForeignKey(y => y.UserId);
        }

    }
}
