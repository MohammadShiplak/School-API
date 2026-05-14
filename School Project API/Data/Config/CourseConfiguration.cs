using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {

            builder.ToTable("Courses");

            builder.HasKey(c => c.Id);

            builder.Property(c=>c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(c=>c.ImagePath)
                .HasMaxLength(500);

            builder.HasData(LoadCourses());

        }


        private static List<Course> LoadCourses()
        {
            return new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "Full Stack Development",
                    Price = 199.99m,
                    ImagePath = "/images/fullstack.png"
                },
                new Course
                {
                    Id = 2,
                    Name = "Database Design",
                    Price = 149.50m,
                    ImagePath = "/images/database.png"
                },
                new Course
                {
                    Id = 3,
                    Name = "Cloud Computing Basics",
                    Price = 99.99m,
                    ImagePath = "/images/cloud.png"
                }
            };
        }




    }
}
