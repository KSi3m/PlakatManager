namespace ElectionMaterialManager.CQRS
{
    public class GenericResponse<T> : Response
    {
        public T Data { get; set; } 
    }
}
