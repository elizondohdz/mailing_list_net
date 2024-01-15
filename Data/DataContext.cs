using mailing_list_net.Models;
using Microsoft.EntityFrameworkCore;
namespace mailing_list_net.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { 
            
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupContact> GroupContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupContact>()
                .HasKey(gc => new { gc.ContactId, gc.GroupId });
            modelBuilder.Entity<GroupContact>()
                .HasOne(c => c.Contact)
                .WithMany(gc => gc.GroupContacts)
                .HasForeignKey(c => c.ContactId);
            modelBuilder.Entity<GroupContact>()
                .HasOne(g => g.Group)
                .WithMany(gc => gc.GroupsContacts)
                .HasForeignKey(c => c.GroupId);
        }

    }
}
