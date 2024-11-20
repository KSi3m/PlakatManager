namespace ElectionMaterialManager.CQRS
{
    public class GenericResponseWithList<T> : Response
    {
        public IEnumerable<T> Data { get; set; }
    }
}
