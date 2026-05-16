using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School_Project_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "UsertId",
                table: "Users",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Email", "PasswordHash", "Role" },
                values: new object[] { "admin@gmail.com", "$2a$11$c.4Cmj3rP.Zrpq9PD0ZZveE/aMFE504E9hxqbVVWKesvuDwElKksu", "Admin" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "Email", "PasswordHash", "Role", "UserName" },
                values: new object[] { "teacher1@gmail.com", "$2a$11$BwOhy/TISJ6FY796dT0I.eN5SALsWuTs6JRp9sRtD0bpXcwEYojtS", "Teacher", "teacher1" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "Email", "PasswordHash", "Role", "UserName" },
                values: new object[] { "teacher2@gmail.com", "$2a$11$AnlrDcQ1SMmjaQd9WMr8RO1/IC2g9fUdx9TOAMzorCCC7D/OcMHLa", "Teacher", "teacher2" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "IsActive", "PasswordHash", "Role", "UserName" },
                values: new object[] { 4, "student1@gmail.com", true, "$2a$11$3Q7ktMT/zai/u.w7PHB7h.2h28yrTIKaLnLDaB0kYfQqAr2ZUSevK", "Student", "student1" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "PasswordHash", "Role", "UserName" },
                values: new object[] { 5, "student2@gmail.com", "$2a$11$vSO5US.IN.S5qC.o4Xy91eU9YnOJ/zhPhEE8hqXP9O1/kgqJv3I4S", "Student", "student2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "UsertId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UsertId",
                keyValue: 1,
                column: "Password",
                value: "Admin@123");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UsertId",
                keyValue: 2,
                columns: new[] { "Password", "UserName" },
                values: new object[] { "Teach@123", "teacher" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UsertId",
                keyValue: 3,
                columns: new[] { "Password", "UserName" },
                values: new object[] { "Stud@123", "student" });
        }
    }
}
