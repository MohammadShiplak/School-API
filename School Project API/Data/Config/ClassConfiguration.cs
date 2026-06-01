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

          
    new Class { Id = 1, Name = "Class A", Capacity = 30, Description = "Backend", TeacherId = 1 },
    new Class { Id = 2, Name = "Class B", Capacity = 25, Description = "Frontend", TeacherId = 2 },
    new Class { Id = 3, Name = "Class C", Capacity = 20, Description = "HR", TeacherId = 3 },
    new Class { Id = 4, Name = "Class D", Capacity = 35, Description = "Finance", TeacherId = 4 },
    new Class { Id = 5, Name = "Class E", Capacity = 40, Description = "Marketing", TeacherId = 5 },
    new Class { Id = 6, Name = "Class F", Capacity = 28, Description = "Sales", TeacherId = 6 },
    new Class { Id = 7, Name = "Class G", Capacity = 32, Description = "Operations", TeacherId = 7 },
    new Class { Id = 8, Name = "Class H", Capacity = 18, Description = "Security", TeacherId = 8 },
    new Class { Id = 9, Name = "Class I", Capacity = 22, Description = "AI", TeacherId = 9 },
    new Class { Id = 10, Name = "Class J", Capacity = 26, Description = "Networking", TeacherId = 10 }



            };


        
        }



    }
}
