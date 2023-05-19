using Student.Domain.CommonModel;
using Student.Domain.Pagination.Filter;
using Student.Domain.Pagination.Helper;
using Student.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Student.Services.Interface;
using Student.Domain.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Student.Services.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly StudentDataContext _context;
        private readonly IUriService _uriService;

        public StudentService(StudentDataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        public async Task<ApiResponse> SaveAsync(Domain.Model.Student obj)
        {
            try
            {
                await _context.AddAsync(obj);
                var response = await _context.SaveChangesAsync();
                if (response > 0)
                {
                    return new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "Data Saved successfully.",
                        Data = obj.Id
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "",
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "Failed",
                    StatusCode = 400,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }
        public async Task<ApiResponse> GetByIdAsync(int Id)
        {
            try
            {
                var DirectorList = await _context.students.Where(x => x.Id == Id).ToListAsync();
                if (DirectorList != null)
                {
                    return new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "get data",
                        Data = DirectorList
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 401,
                        Message = "No Data found!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "Failed",
                    StatusCode = 400,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }
        public async Task<ApiResponse> GetAllAsync()
        {
            try
            {
                var item = await _context.students.ToListAsync();
                return new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = item
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "Failed",
                    StatusCode = 400,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<ApiResponse> GetAllAsync(PaginationFilter model, string route)
        {
            try
            {

                var item = await _context.students.Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync();
                var totalRecords = await _context.students.CountAsync();
                var pagedReponse = PaginationHelper.CreatePagedReponse<Domain.Model.Student>(item, model, totalRecords, _uriService, route);

                return new ApiResponse()
                {
                    Status = "Success",
                    StatusCode = 200,
                    Message = "get data",
                    Data = pagedReponse
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateAsync(int id, Domain.Model.Student obj)
        {
            try
            {
                var oldData = await _context.students.FirstOrDefaultAsync(x => x.Id == id);
                if (oldData == null)
                {
                    return new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "Not found",
                        Data = null
                    };
                }
                oldData.Name = obj.Name;
                oldData.Address = obj.Address;
                oldData.Roll=obj.Roll;
                _context.students.Update(oldData);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new ApiResponse()
                    {
                        Status = "Success",
                        StatusCode = 200,
                        Message = "updated",
                        Data = oldData
                    };
                }
                else
                {
                    return new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "unknown",
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "Failed",
                    StatusCode = 400,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }
        public async Task<ApiResponse> DeleteAsync(long id)
        {
            try
            {
                var oldData = await _context.students.FirstOrDefaultAsync(x => x.Id == id);
                if (oldData == null)
                {
                    return new ApiResponse()
                    {
                        Status = "Failed",
                        StatusCode = 400,
                        Message = "Data not found",
                        Data = null
                    };
                }
                else
                {
                    _context.students.Remove(oldData);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return new ApiResponse()
                        {
                            Status = "Success",
                            StatusCode = 200,
                            Message = "successfully deleted",
                            Data = null
                        };
                    }
                    else
                    {
                        return new ApiResponse()
                        {
                            Status = "Failed",
                            StatusCode = 400,
                            Message = "Internal problem",
                            Data = null
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "Failed",
                    StatusCode = 400,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }
    }
}
