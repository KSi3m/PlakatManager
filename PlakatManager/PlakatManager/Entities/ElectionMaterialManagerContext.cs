using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ElectionMaterialManager.Entities
{
    public class ElectionMaterialManagerContext: IdentityDbContext<IdentityUser>
    {
        public  DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ElectionItem> ElectionItems { get; set; }
        public DbSet<Billboard> Billboards { get; set; }
        public DbSet<Poster> Poster { get; set; }
        public DbSet<LED> Leds { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<ElectionItemTag> ElectionItemTag { get; set; }

        public ElectionMaterialManagerContext(DbContextOptions<ElectionMaterialManagerContext> options):base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.Name.ToLower());
                }
            }


            modelBuilder.Entity<Billboard>(eb => {
                eb.Property(x => x.StartDate).HasColumnName("start_date");
                eb.Property(x => x.EndDate).HasColumnName("end_date");
            }); 

            modelBuilder.Entity<Poster>(eb => {
                eb.Property(x => x.PaperType).HasColumnName("paper_type").HasMaxLength(50);
            });

            modelBuilder.Entity<LED>(eb =>
            {
                eb.Property(x => x.RefreshRate).HasColumnName("refresh_rate");
            });

            modelBuilder.Entity<ElectionItem>(eb =>
            {
                eb.Property(x => x.Size).HasColumnType("nvarchar(20)");
                eb.Property(x => x.Cost).HasPrecision(10,4);

                eb.OwnsOne(x => x.Location, cmb =>
                {
                    cmb.Property(x=>x.Latitude).HasPrecision(10,5);
                    cmb.Property(x=>x.Longitude).HasPrecision(10,5);
      
                });


                eb.HasOne(e => e.Status)
                .WithMany(s => s.ElectionItems)
                .HasForeignKey(e => e.StatusId);

                eb.HasMany(ei => ei.Comments)
                .WithOne(c => c.ElectionItem)
                .HasForeignKey(c => c.ElectionItemId);

                eb.HasOne(ei => ei.Author)
                .WithMany(u => u.ElectionItems)
                .HasForeignKey(ei => ei.AuthorId);


                eb.HasMany(e => e.Tags)
                .WithMany(t => t.ElectionItems)
                .UsingEntity<ElectionItemTag>(
                    t => t.HasOne(et =>et.Tag)
                    .WithMany()
                    .HasForeignKey(et => et.TagId),

                     e => e.HasOne(et => et.ElectionItem)
                    .WithMany()
                    .HasForeignKey(et => et.ElectionItemId),

                     et =>
                     {
                         et.HasKey(x => new { x.TagId, x.ElectionItemId });
                         et.Property(x => x.DateOfPublication)
                           .HasDefaultValueSql("getutcdate()")
                           .HasColumnName("date_of_publication");
                     });

                    
            });

            modelBuilder.Entity<Status>().Property(x => x.Name).IsRequired().HasMaxLength(30);

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(x => x.CreatedAt).HasDefaultValueSql("getutcdate()");
                eb.Property(x => x.UpdatedAt).ValueGeneratedOnUpdate();

                eb.HasOne(x => x.Author)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<User>()
                .HasOne(x => x.Address)
                .WithOne(x => x.User)
                .HasForeignKey<Address>(x => x.UserId);
                
                
        }
    }
}
