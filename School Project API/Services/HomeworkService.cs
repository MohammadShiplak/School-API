using Microsoft.EntityFrameworkCore;
using School_Project_API.DTO;
using School_Project_API.Entities;
using School_Project_API.helper;
using School_Project_API.Services.Interfaces;

namespace School_Project_API.Services
{
    public class HomeworkService : IHomeworkService
    {

        private readonly ApplicationDbContext _context;

        public HomeworkService(ApplicationDbContext context)
        {
_context = context; 
        }

     private static HomeworkDTO MapToDTO(Homework homework)
        {
            return new HomeworkDTO
            {
                Id = homework.Id,
                ClassId = homework.ClassId,
                TeacherId = homework.TeacherId,
                Title = homework.Title,
                Description = homework.Description,
                DueDate = homework.DueDate,
                CreatedAt = homework.CreatedAt, 
                Status = homework.Status,


                TeacherName = homework.Teacher != null ? homework.Teacher.Name : null,
                ClassName = homework.Class != null ? homework.Class.Name : null,
                SubjectName = homework.Subject != null ? homework.Subject.SubjectName : null   

            };
        }   

        public async Task<HomeworkDTO> AddHomeworkAsync(HomeworkDTO homeworkDTO)
        {
            var teacherExists = await _context.Teacher.AnyAsync(t => t.Id == homeworkDTO.TeacherId);

            if (!teacherExists)
                throw new InvalidOperationException($"Teacher with Id {homeworkDTO.TeacherId} does not exist.");

            var homework = new Homework
            {
                TeacherId = homeworkDTO.TeacherId,
                ClassId = homeworkDTO.ClassId,
                SubjectId = homeworkDTO.SubjectId,
                Title = homeworkDTO.Title,
                Description = homeworkDTO.Description,
                DueDate = homeworkDTO.DueDate,
                CreatedAt = DateTime.UtcNow, // Set created time on server
                Status = homeworkDTO.Status
            };  

            _context.Homeworks.Add(homework);
            await _context.SaveChangesAsync(); // Save to get the generated Id


            await _context.Entry(homework).Reference(h => h.Teacher).LoadAsync();
            await _context.Entry(homework).Reference(h => h.Class).LoadAsync();
            await _context.Entry(homework).Reference(h => h.Subject).LoadAsync();   

return MapToDTO(homework);  
        }

        public async Task<bool> DeleteHomeworkAsync(int id)
        {
          var homework=await _context.Homeworks.FindAsync(id);

            if (homework == null)
                return false;

            _context.Homeworks.Remove(homework);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResponse<HomeworkDTO>> GetAllHomeworkAsync(int pageNumber, int pageSize)
        {
            var query = _context.Homeworks
                .AsNoTracking()
                .Include(h=>h.Teacher)
                .Include(h=>h.Class)
                .Include(h=>h.Subject)
                .OrderByDescending(h=>h.CreatedAt);

            var totalRecords = await query.CountAsync();


            var homeworkList = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                  .Select(h => new HomeworkDTO
                  {
                      // WHY inline Select here instead of MapToDTO?
                      //   .Select() on IQueryable runs in SQL (server-side).
                      //   MapToDTO(h) runs in C# (client-side) — EF Core
                      //   would first load ALL columns, then map.
                      //   Inline projection = SQL only fetches needed columns.
                      //   TRADE-OFF: Slight code duplication vs. SQL performance.
                      //   For large tables, inline is better. For small tables, MapToDTO is fine.
                      Id = h.Id,
                      TeacherId = h.TeacherId,
                      ClassId = h.ClassId,
                      SubjectId = h.SubjectId,
                      Title = h.Title,
                      Description = h.Description,
                      DueDate = h.DueDate,
                      CreatedAt = h.CreatedAt,
                      Status = h.Status,
                      TeacherName = h.Teacher != null ? h.Teacher.Name : "Unknown",
                      ClassName = h.Class != null ? h.Class.Name : null,
                      SubjectName = h.Subject != null ? h.Subject.SubjectName : null,
                  })
                .ToListAsync();


            return new PagedResponse<HomeworkDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = homeworkList
            };

        }

        public async  Task<List<HomeworkDTO>> GetHomeworkByClassAsync(int classId)
        {
            return await _context.Homeworks
                  .AsNoTracking()
                  .Include(h => h.Teacher)
                  .Include(h => h.Class)
                  .Include(h => h.Subject)
                  .Where(h => h.ClassId == classId && h.Status == HomeworkStatus.Active)
                  .OrderBy(h => h.DueDate) // upcoming first for students
                  .Select(h => new HomeworkDTO
                  {
                      Id = h.Id,
                      TeacherId = h.TeacherId,
                      ClassId = h.ClassId,
                      SubjectId = h.SubjectId,
                      Title = h.Title,
                      Description = h.Description,
                      DueDate = h.DueDate,
                      CreatedAt = h.CreatedAt,
                      Status = h.Status,
                      TeacherName = h.Teacher != null ? h.Teacher.Name : "Unknown",
                      ClassName = h.Class != null ? h.Class.Name : null,
                      SubjectName = h.Subject != null ? h.Subject.SubjectName : null,
                  })
                  .ToListAsync();
        }

        public Task<HomeworkDTO?> GetHomeworkByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<HomeworkDTO>> GetHomeworkByTeacherAsync(int teacherId)
        {
            return await _context.Homeworks
                  .AsNoTracking()
                  .Include(h => h.Teacher)
                  .Include(h => h.Class)
                  .Include(h => h.Subject)
                  .Where(h => h.TeacherId == teacherId)
                  // WHY .Where() before .Select():
                  //   EF Core translates this to SQL WHERE clause.
                  //   The filter happens in SQL (server-side) not in C#.
                  //   = Only matching rows travel over the network.
                  .OrderByDescending(h => h.DueDate)
                  .Select(h => new HomeworkDTO
                  {
                      Id = h.Id,
                      TeacherId = h.TeacherId,
                      ClassId = h.ClassId,
                      SubjectId = h.SubjectId,
                      Title = h.Title,
                      Description = h.Description,
                      DueDate = h.DueDate,
                      CreatedAt = h.CreatedAt,
                      Status = h.Status,
                      TeacherName = h.Teacher != null ? h.Teacher.Name : "Unknown",
                      ClassName = h.Class != null ? h.Class.Name : null,
                      SubjectName = h.Subject != null ? h.Subject.SubjectName : null,
                  })
                  .ToListAsync();
        }

        public async  Task<HomeworkDTO?> UpdateHomeworkAsync(int id, HomeworkDTO homeworkDTO)
        {
            // WHY Include here: We need to return TeacherName etc. in the response.
            // WHY not FindAsync: FindAsync doesn't support .Include().
            var homework = await _context.Homeworks
                .Include(h => h.Teacher)
                .Include(h => h.Class)
                .Include(h => h.Subject)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (homework == null)
                return null;

            // Only update fields the client is allowed to change.
            // WHY not update TeacherId or CreatedAt:
            //   A homework's author shouldn't change after creation.
            //   CreatedAt is immutable — it's a historical record.
            homework.Title = homeworkDTO.Title;
            homework.Description = homeworkDTO.Description;
            homework.DueDate = homeworkDTO.DueDate;
            homework.ClassId = homeworkDTO.ClassId;
            homework.SubjectId = homeworkDTO.SubjectId;
            homework.Status = homeworkDTO.Status;

            // WHY no _context.Update() call:
            //   EF Core is TRACKING this entity (we loaded it above without AsNoTracking).
            //   It automatically detects property changes (Change Tracking).
            //   SaveChangesAsync() generates SQL UPDATE only for changed columns.
            await _context.SaveChangesAsync();

            return MapToDTO(homework);
        }
    }
}
