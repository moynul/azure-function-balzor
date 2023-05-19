using Newtonsoft.Json;
using Student.UI.Data;
using Student.UI.DTOModel;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Student.UI.Services
{
    public class StudentService : IStudentService
    {
        public HttpClient _httpClient { get; }
        private readonly IConfiguration _configuration;
        public StudentService(HttpClient httpClient, IConfiguration configuration)
        {

            _httpClient = httpClient;
            _configuration = configuration;
            var url = _configuration["AppSettings:BaseAddress"];
            _httpClient.BaseAddress = new Uri(url);

        }
        public async Task<ApiResponse<List<StudentDTO>>> GetAllUserList()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "GetDirectorAsync");
            var response = await _httpClient.SendAsync(requestMessage);
            var responseBody = await response.Content.ReadAsStringAsync();
            var responseStatusCode = response.StatusCode.ToString();
            var returnedUser = new ApiResponse<List<StudentDTO>>();

            if (responseStatusCode == "OK")
            {
                returnedUser = JsonConvert.DeserializeObject<ApiResponse<List<StudentDTO>>>(responseBody);
                return returnedUser;
            }

            var errorRes = await ErrorMethod(responseStatusCode, responseBody);
            returnedUser.Status = errorRes.Status;
            returnedUser.StatusCode = errorRes.StatusCode;
            returnedUser.Message = errorRes.Message;
            return returnedUser;

        }

        public async Task<ApiResponse> ErrorMethod(string statusCode, string responseBody)
        {
            ApiResponse response = new ApiResponse();
            if (statusCode == "Unauthorized")
            {
                response.Status = "Error";
                response.StatusCode = 401;
                response.Message = "Refresh token is expire or unauthorized access";
            }
            else if (statusCode == "BadRequest")
            {
                var data = JsonConvert.DeserializeObject<BadRequestErrorModel>(responseBody);
                response = new ApiResponse()
                {
                    Status = "invalid_grant",
                    StatusCode = 400,
                    Message = data.error_description
                };
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(responseBody))
                    response = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                else
                {
                    response.Status = statusCode;
                    response.StatusCode = 404;
                    response.Message = "Not Found.";
                }
            }

            return response;
        }

        public async Task<ApiResponse<PaginationModel<List<StudentDTO>>>> GetAllAsync(string requestUri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            if (responseStatusCode.ToString() == "OK")
            {
                return await Task.FromResult(JsonConvert.DeserializeObject<ApiResponse<PaginationModel<List<StudentDTO>>>>(responseBody));
            }
            var data = await ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse<PaginationModel<List<StudentDTO>>>()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }

        public async Task<ApiResponse> SaveAsync(string requestUri, StudentDTO obj)
        {
            string serializedUser = JsonConvert.SerializeObject(obj);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();
            if (responseStatusCode == "OK")
            {
                var returnedObj = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return await Task.FromResult(returnedObj);
            }
            var data = await ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }

        public async Task<ApiResponse> UpdateSaveAsync(string requestUri, StudentDTO obj)
        {
            string serializedUser = JsonConvert.SerializeObject(obj);

            var requestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);


            requestMessage.Content = new StringContent(serializedUser);

            requestMessage.Content.Headers.ContentType
                = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();

            if (responseStatusCode == "OK")
            {
                var returnedObj = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return await Task.FromResult(returnedObj);
            }
            var data = await ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }

        public async Task<ApiResponse> DeleteAsync(string requestUri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode.ToString();
            var responseBody = await response.Content.ReadAsStringAsync();

            if (responseStatusCode == "OK")
            {
                var returnedObj = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return await Task.FromResult(returnedObj);
            }
            var data = await ErrorMethod(responseStatusCode, responseBody);
            return new ApiResponse()
            {
                Status = data.Status,
                StatusCode = data.StatusCode,
                Message = data.Message
            };
        }

        private string Get_Https_result4_GET_Method(string destinationUrl)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string certPath = AppDomain.CurrentDomain.BaseDirectory + "certificate.pfx";
            X509Certificate cert = X509Certificate.CreateFromCertFile(certPath);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);

            request.Method = "GET";
            request.PreAuthenticate = true;
            request.ClientCertificates.Add(cert);
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, SslPolicyErrors) => true;
            dynamic jsonResult = new object();
            HttpWebResponse response;
            string returnMessage;

            try
            {
                if (request != null)
                {
                    response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream responseStream = response.GetResponseStream();
                        jsonResult = JsonConvert.DeserializeObject(new StreamReader(responseStream).ReadToEnd());
                        return System.Convert.ToString(jsonResult);
                    }
                    else
                    {
                        returnMessage = response.StatusCode + " " + response.StatusDescription;

                        return returnMessage;
                    }
                }
            }
            catch (WebException ex)
            {
                string message = string.Empty;
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var error = JsonConvert.DeserializeObject(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                    return System.Convert.ToString(error);
                }
                else
                {
                    message = ex.Status.ToString();
                }
                return message;
            }
            return System.Convert.ToString(jsonResult);
        }
    }
}
