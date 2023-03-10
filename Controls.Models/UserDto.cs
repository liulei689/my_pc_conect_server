using System.ComponentModel.DataAnnotations.Schema;
namespace Controls.Net7.Api.Model.Dto
{
    [Table("usermanger")]
    public class UserDto:User
    {
        public string? Role { get; set; }

    }
    public class User 
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    
    }
}
