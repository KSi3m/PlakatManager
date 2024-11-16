using System.ComponentModel.DataAnnotations;

namespace ElectionMaterialManager.Dtos
{
    public class LoginRequestDTO
    {

        public string Login { get; set; }
        public string Password { get; set; }

    }
}
