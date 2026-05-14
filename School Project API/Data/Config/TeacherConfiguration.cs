using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School_Project_API.Entities;
using System.Reflection.Metadata.Ecma335;

namespace School_Project_API.Data.Config
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(250);

            

            builder.Property(c => c.Specialization)
                   .IsRequired();

            builder.Property(c => c.HireDate)
                   .IsRequired();


            /*
             1-M one department has many teachers
            */

            builder.HasOne(d => d.Department)
                .WithMany(t => t.Teachers)
                .HasForeignKey(f => f.DepartmentId);












            builder.ToTable("Teachers");

            // Use fixed dates for seed data to avoid EF thinking it's always changed
            builder.HasData(
             

            );
        }
    }




}
