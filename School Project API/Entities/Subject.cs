using System.Text.Json.Serialization;

namespace School_Project_API.Entities
{
    public class Subject
    {

        public int Id { get; set; } 

        public int CourseId { get; set; }

        public int TeacherId { get; set; }  
        public string ?SubjectName { get; set; }    


        public decimal Price { get; set; }

        public Course Course { get; set; }

        public Teacher Teacher { get; set; }    

        public ICollection<Student> Student { get; set; }  = new List<Student>();  

        
    }
}
