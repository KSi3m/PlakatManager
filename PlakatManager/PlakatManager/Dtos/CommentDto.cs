using ElectionMaterialManager.Entities;

namespace ElectionMaterialManager.Dtos
{
    public class CommentDto
    {
        public string Message { get; set; }
        public AuthorDto Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
