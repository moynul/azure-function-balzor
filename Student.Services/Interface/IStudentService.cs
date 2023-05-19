using Student.Domain.CommonModel;
using Student.Domain.Pagination.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Services.Interface
{
    public interface IStudentService
    {
        Task<ApiResponse> SaveAsync(Domain.Model.Student obj);
        Task<ApiResponse> GetByIdAsync(int Id);
        Task<ApiResponse> GetAllAsync();
        Task<ApiResponse> GetAllAsync(PaginationFilter model, string route);
        Task<ApiResponse> UpdateAsync(int id, Domain.Model.Student obj);
        Task<ApiResponse> DeleteAsync(long id);
    }
}
