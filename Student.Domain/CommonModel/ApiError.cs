using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Domain.CommonModel
{
    public class ApiError
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public string details { get; set; }
    }
}
