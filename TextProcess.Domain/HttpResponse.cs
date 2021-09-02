namespace TextProcess.Domain
{
    public class HttpResponse<T>
    {
        public T Data { get; set; }

        public bool IsSuccess { get; set; }

        public string Error { get; set; }
    }
}
