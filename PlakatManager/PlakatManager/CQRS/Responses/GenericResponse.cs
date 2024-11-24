namespace ElectionMaterialManager.CQRS.Responses
{
    public class GenericResponse<T> : Response
    {
        public T Data { get; set; }
    }
}
