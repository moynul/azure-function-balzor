using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Domain.Pagination.Wreppers
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data)
        {
            Item = data;
        }
        public T Item { get; set; }
    }
}
