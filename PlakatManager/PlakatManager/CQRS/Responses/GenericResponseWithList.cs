namespace ElectionMaterialManager.CQRS.Responses
{
    public class GenericResponseWithList<T> : Response
    {
        public IEnumerable<T> Data { get; set; }
    }
}
