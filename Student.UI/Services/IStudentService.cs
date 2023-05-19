using Student.UI.Data;
using Student.UI.DTOModel;

namespace Student.UI.Services
{
    public interface IStudentService
    {
        Task<ApiResponse<List<StudentDTO>>> GetAllUserList();
        Task<ApiResponse<PaginationModel<List<StudentDTO>>>> GetAllAsync(string requestUri);
        Task<ApiResponse> SaveAsync(string requestUri, StudentDTO obj);
        Task<ApiResponse> UpdateSaveAsync(string requestUri, StudentDTO obj);
        Task<ApiResponse> ErrorMethod(string statusCode, string responseBody);
        Task<ApiResponse> DeleteAsync(string requestUri);
    }
}
