namespace ElectionMaterialManager.CQRS.Responses
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public IEnumerable<string> Errors { get; set; } 

    }
}
