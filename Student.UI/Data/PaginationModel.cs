namespace Student.UI.Data
{
    public class PaginationModel
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string firstPage { get; set; }
        public string lastPage { get; set; }
        public int totalPages { get; set; }
        public int totalRecords { get; set; }
        public string nextPage { get; set; }
        public string previousPage { get; set; }
        public object item { get; set; }
    }

    public class PaginationModel<T>
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string firstPage { get; set; }
        public string lastPage { get; set; }
        public int totalPages { get; set; }
        public int totalRecords { get; set; }
        public string nextPage { get; set; }
        public string previousPage { get; set; }
        public T item { get; set; }
    }
}
