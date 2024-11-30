namespace ElectionMaterialManager.Dtos
{
    public class UserCommentDto
    {
        public int ElectionItemId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
