using System.Text.Json.Serialization;

namespace School_Project_API.Entities
{
    public class Department
    {

        public int Id { get; set; }

        public string Name { get; set; }



        public ICollection<Teacher> Teachers { get; set; } =new List<Teacher>();     



       public ICollection<Student> Students { get; set; } =new List<Student>();   


    }
}
