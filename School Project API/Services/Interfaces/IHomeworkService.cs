using School_Project_API.DTO;
using School_Project_API.helper;

namespace School_Project_API.Services.Interfaces
{
    public interface IHomeworkService
    {
        Task<HomeworkDTO?> GetHomeworkByIdAsync(int id);
        Task<PagedResponse<HomeworkDTO>> GetAllHomeworkAsync(int pageNumber, int pageSize);
        Task<List<HomeworkDTO>> GetHomeworkByTeacherAsync(int teacherId);
        Task<List<HomeworkDTO>> GetHomeworkByClassAsync(int classId);

        Task<HomeworkDTO> AddHomeworkAsync(HomeworkDTO homeworkDTO);
        Task<HomeworkDTO?> UpdateHomeworkAsync(int id, HomeworkDTO homeworkDTO);
        Task<bool> DeleteHomeworkAsync(int id); 
    }
}
