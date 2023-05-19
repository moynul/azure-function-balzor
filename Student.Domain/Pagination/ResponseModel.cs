using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Student.Domain.Pagination
{
    public class ResponseModel
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
    public class ResponseModel<T> : ResponseModel
    {
        private readonly bool enablePaging;
        private readonly int pageIndex,
                             pageSize;

        public ResponseModel(bool enablePaging = false, int pageIndex = 0, int pageSize = 10)
        {
            this.enablePaging = enablePaging;
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
        }

        public T Response { get; set; }
        private IEnumerable<T> responseList;
        public IEnumerable<T> ResponseList
        {
            set
            {
                responseList = value;
                this.TotalRecords = responseList.Count();
            }
            get
            {
                if (enablePaging)
                {
                    responseList = responseList.Skip(pageIndex * pageSize).Take(pageSize);
                }
                return responseList;
            }
        }

        public int TotalRecords { get; set; }
    }
    public class ResponseModel<M, D> : ResponseModel
    {
        private readonly bool enablePaging;
        private readonly int pageIndex,
                             pageSize;

        public ResponseModel(bool enablePaging = false, int pageIndex = 0, int pageSize = 10)
        {
            this.enablePaging = enablePaging;
            this.pageIndex = pageIndex;
            this.pageSize = pageSize;
        }

        public M Response { get; set; }
        private IEnumerable<D> responseList;
        public IEnumerable<D> ResponseList
        {
            set
            {
                responseList = value;
            }
            get
            {
                if (enablePaging)
                {
                    responseList = responseList.Skip(pageIndex).Take(pageSize);
                }
                return responseList;
            }
        }
    }
}
