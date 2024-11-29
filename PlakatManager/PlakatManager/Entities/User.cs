using Microsoft.AspNetCore.Identity;

namespace ElectionMaterialManager.Entities
{
    public class User : IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName {  get; set; }
       
        public Address Address { get; set; }

        public List<ElectionItem> ElectionItems { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];

    }
}
