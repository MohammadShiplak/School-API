using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using School_Project_API.Entities;


public class ApplicationDbContext: DbContext
    {

       

        public DbSet<Student>Students { get; set; }
        public DbSet<AccessCard> AccessCards { get; set; }
      
       public DbSet<Department>Departments { get; set; }   

    public DbSet<Class> Class { get; set; }

    public DbSet<Teacher> Teacher { get; set; }

    public DbSet<Course> Course { get; set; }
       public DbSet<Subject> Subjects { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {


        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);


            var Config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var ConnectionString = Config.GetSection("Constr").Value;


            optionsBuilder.UseSqlServer(ConnectionString);


        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }



















    }

