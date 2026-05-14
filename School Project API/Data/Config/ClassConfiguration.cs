using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Capacity).IsRequired();

            builder.Property(c => c.Description)
                .HasMaxLength(500)
                .HasDefaultValue("No Description");

            builder.HasOne(t => t.Teacher)
                .WithMany(c => c.Class)
                .HasForeignKey(f => f.TeacherId);





            builder.ToTable("Classes");


            builder.HasData(LoadClasses());


        }


        public static List<Class> LoadClasses()
        {

            return new List<Class>()
            {

          


            };


        
        }



    }
}
