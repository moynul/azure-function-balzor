using System.IO;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Student.Domain.Pagination.Filter;
using Student.Services.Interface;

namespace Student.Functions
{
    public class StudentFunction
    {
        private readonly ILogger _logger;
        private readonly IStudentService _iStudentService;

        public StudentFunction(ILoggerFactory loggerFactory, IStudentService iStudentService)
        {
            _logger = loggerFactory.CreateLogger<StudentFunction>();
            _iStudentService = iStudentService;
        }

        [Function("GetStudentAsync")]
        public async Task<HttpResponseData> GetStudentAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetStudentAsync")] HttpRequestData req, int PageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("Get GetStudentAsync HTTP trigger function processed a request.");
                PaginationFilter model = new PaginationFilter();
                model.PageSize = pageSize;
                model.PageNumber = PageNumber;

                var postsArray = await _iStudentService.GetAllAsync(model, "GetStudentAsync");
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(postsArray);
                return response;
            }
            catch (Exception ex)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync("Error: " + ex.Message.ToString());
                return response;
            }
        }

        //http://localhost:7260/api/GetStudentByIDAsync/2       
        [Function("GetStudentByIDAsync")]
        public async Task<HttpResponseData> GetStudentByIDAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetStudentByIDAsync/{id:int?}")] HttpRequestData req, int id)
        {
            try
            {
                _logger.LogInformation("Get GetStudentByIDAsync HTTP trigger function processed a request.");
                var postsArray = await _iStudentService.GetByIdAsync(id);
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(postsArray);
                return response;
            }
            catch (Exception ex)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync("Error: " + ex.Message.ToString());
                return response;
            }
        }

        [Function("CreateStudent")]
        public async Task<HttpResponseData> CreateStudent([HttpTrigger(AuthorizationLevel.Function, "post", Route = "CreateStudent")] HttpRequestData req)
        {
            var directorJson = await req.ReadAsStringAsync();
            try
            {
                _logger.LogInformation("CreateStudent HTTP trigger function processed a request.");
                var director = JsonConvert.DeserializeObject<Domain.Model.Student>(directorJson);
                var entity = await _iStudentService.SaveAsync(director);
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(entity);
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to create a Student: {directorJson}";
                _logger.LogInformation(errorMessage);
                _logger.LogInformation("Error Message: " + ex.Message.ToString());

                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync("Error: " + ex.Message.ToString());
                return response;
            }
        }

        [Function("UpdateStudentAsync")]
        public async Task<HttpResponseData> UpdateStudentAsync([HttpTrigger(AuthorizationLevel.Function, "put", Route = "UpdateStudentAsync/{id:int?}")] HttpRequestData req, int id)
        {
            var directorJson = await req.ReadAsStringAsync();
            try
            {
                _logger.LogInformation("UpdateStudentAsync HTTP trigger function processed a request.");
                var director = JsonConvert.DeserializeObject<Domain.Model.Student>(directorJson);

                var entity2 = await _iStudentService.UpdateAsync(id, director);
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(entity2);
                return response;

            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to Update Student: {directorJson}";
                _logger.LogInformation(errorMessage);
                _logger.LogInformation("Error Message: " + ex.Message.ToString());

                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync("Error: " + ex.Message.ToString());
                return response;
            }
        }

        [Function("DeleteStudentAsync")]
        public async Task<HttpResponseData> DeleteStudentAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "DeleteStudentAsync/{id:int?}")] HttpRequestData req, int id)
        {
            try
            {
                _logger.LogInformation("DeleteStudentAsync HTTP trigger function processed a request.");
                var postsArray = await _iStudentService.DeleteAsync(id);
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteAsJsonAsync(postsArray);
                return response;
            }
            catch (Exception ex)
            {
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteAsJsonAsync("Error: " + ex.Message.ToString());
                return response;
            }
        }
    }
}
