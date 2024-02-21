namespace RazorPages.Model
{
    public class Result<T>
    {
        public T? Data { get; set; }
        public bool isSuccess { get; set; }
        public string Message { get; set; }
    }
}