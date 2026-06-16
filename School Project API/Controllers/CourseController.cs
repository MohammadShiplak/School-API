using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School_Project_API.DTO;
using School_Project_API.helper;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {

        private readonly ICourseService _courstService;

        public CourseController(ICourseService courstService)
        {
            _courstService = courstService;
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<CourseDTO>> GetCourseByID(int id)
        {

            if (id < 0)
                return BadRequest($"Id '{id}' is not valid. Id must be a positive number");

            var course = await _courstService.GetCourseByIdAsync(id);
            if (course == null)
                return NotFound($"Student with Id {id} was not found");

            return Ok(course);




        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<CourseDTO>>> GetAllCourses(int pageNumber = 1, int pageSize = 10)
        {

            var courses = await _courstService.GetAllCourseAsync(pageNumber,pageSize);
            return Ok(courses);


        }



        /*
         
        in this case 


        it is so significant to create a  DTO in order to send required information to server 

        and void unNecessary information 


        */


        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<CourseDTO>> AddCourse(CourseDTO course)
        {

            var createdStudent = await _courstService.AddCourseAsync(course); ;

            return CreatedAtAction(nameof(GetCourseByID), new { id = createdStudent.Id }, createdStudent);



        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<StudentDTO>> Updatecourses(int id, CourseDTO courseDTO)
        {

            var updatedstudent = await _courstService.UpdateCourseAsync(id,courseDTO);

            if (updatedstudent == null)
                return NotFound($"course with Id {id} was not found");

            return Ok(updatedstudent);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var deleted = await _courstService.DeleteCourseAsync(id);

            if (!deleted)
                return NotFound($"course with Id {id} was not found");


            return Ok($"course with Id {id} deleted successfully");
        }

















    }
}
