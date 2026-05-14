using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context; 
        }

        private static DepartmentDTO MapToDto(Department department)
        {

            return new DepartmentDTO
            {

                Id = department.Id,
              Name = department.Name,
            };


        }






        public async Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO departmentDTO)
        {


            var NewDepartment = new Department
            {

             Id= departmentDTO.Id,  
             Name = departmentDTO.Name, 
            };


            _context.Departments.Add(NewDepartment);

            await _context.SaveChangesAsync();





            return MapToDto(NewDepartment);
        }

        public async Task<bool> DeleteDepartemntAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
                return false;

            _context.Departments.Remove(department);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
        {
            return await _context.Departments.AsNoTracking()
                 .Select(d => new DepartmentDTO
                 {

                     Id = d.Id,
                     Name = d.Name,

                 }).ToListAsync();
        }

        public async Task<DepartmentDTO> GetDepartmentByIdAsync(int? id)
        {
        var department=await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                return null;

            return MapToDto(department);

        }

        public async Task<DepartmentDTO> UpdateDepartmentAsync(int id, DepartmentDTO departmentDTO)
        {

            var department =await  _context.Departments.FindAsync(id);

            department.Name=departmentDTO.Name;


          await _context.SaveChangesAsync();


            return MapToDto(department);







        }
    }
}
