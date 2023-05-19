namespace Student.UI.Data
{
    public class ApiResponse
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class ApiResponse<T>
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class Errors
    {
        public List<string> Role { get; set; }
        public List<string> Password { get; set; }
        public List<string> PhoneNumber { get; set; }
    }

    public class ValidationErrorModel
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string traceId { get; set; }
        public Errors errors { get; set; }
    }
}
