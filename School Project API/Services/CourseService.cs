using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.helper;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Services
{
    public class CourseService : ICourseService
    {

        private readonly ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;

        }



        private CourseDTO MapToDto(Course newcourse)
        {

            return new CourseDTO
            {
                Id = newcourse.Id,  
                Name = newcourse.Name,  
                Price = newcourse.Price,    
                ImagePath = newcourse.ImagePath,
            };


        }







        public async Task<CourseDTO> AddCourseAsync(CourseDTO DTO)
        {
            var NewCourse = new Course
            {

                Id = DTO.Id,
                Name = DTO.Name,
                Price = DTO.Price,
                ImagePath = DTO.ImagePath,
            };


            _context.Course.Add(NewCourse);

            await _context.SaveChangesAsync();
            return MapToDto(NewCourse);
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var Id = await _context.Course.FindAsync(id);

            if (Id == null)
                return false;

            _context.Course.Remove(Id);

            await _context.SaveChangesAsync();

            return true;
        }

        public  async Task<PagedResponse<CourseDTO>> GetAllCourseAsync(int pageNumber,int pageSize)
        {

            var query = _context.Course.AsNoTracking();


            var totalRecords = await query.CountAsync();


            var courses = await _context.Course

.OrderBy(s => s.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)

        .Select(d => new CourseDTO
        {
            Id = d.Id,
            Name = d.Name,
            Price = d.Price,    
            ImagePath = d.ImagePath,
       

        })
        .ToListAsync();

            return new PagedResponse<CourseDTO>
            {

                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling
               (
                    totalRecords / (double)pageSize
                ),

                Data = courses

            };
        }

        public async Task<CourseDTO> GetCourseByIdAsync(int id)
        {
            var course = await _context.Course.
               FirstOrDefaultAsync(s => s.Id == id);



            if (course == null)
                return null;


            return MapToDto(course); ;
        }

        public async Task<CourseDTO> UpdateCourseAsync(int id, CourseDTO courseDTO)
        {
            var course = await _context.Course.FindAsync(id);

            if (course == null)
                return null;


            course.Name = courseDTO.Name;
            course.Price = courseDTO.Price;
            course.ImagePath = courseDTO.ImagePath;
          




            // Save changes and get the updated student
            await _context.SaveChangesAsync();

            return MapToDto(course);

        }

   
        
    }
}
