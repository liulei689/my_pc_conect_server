﻿using System.ComponentModel.DataAnnotations.Schema;
namespace Controls.Net7.Api.Model.Dto
{
    [Table("usermanger")]
    public class UserDto
    {
       public string? Password {get;set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }

    }
}
