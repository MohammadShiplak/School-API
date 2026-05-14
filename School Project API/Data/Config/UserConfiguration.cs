using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using School_Project_API.Entities;

namespace School_Project_API.Data.Config
{

        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                // Table name
                builder.ToTable("Users");

                // Primary key
                builder.HasKey(u => u.UsertId);

                // UserName column
                builder.Property(u => u.UserName)
                       .HasMaxLength(50)    // VARCHAR(50)
                       .IsRequired();       // NOT NULL

                // Password column
                builder.Property(u => u.Password)
                       .HasMaxLength(100)   // VARCHAR(100)
                       .IsRequired();       // NOT NULL

                // IsActive column
                builder.Property(u => u.IsActive)
                       .HasDefaultValue(true); // Default: true


            builder.HasData(
         new User { UsertId = 1, UserName = "admin", Password = "Admin@123", IsActive = true },
         new User { UsertId = 2, UserName = "teacher", Password = "Teach@123", IsActive = true },
         new User { UsertId = 3, UserName = "student", Password = "Stud@123", IsActive = false }
     );



        }
        }

    }

