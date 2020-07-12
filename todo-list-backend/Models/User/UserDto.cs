using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.User
{
    public class UserDto
    {
		public UserDto(UserRecord record)
        {
			Id = record.Id;
			Email = record.Email;
			DisplayName = record.DisplayName;
			SsoUser = record.SsoUser;
        }

		public int Id { get; set; }
		public string Email { get; set; }
		public string DisplayName { get; set; }
		public bool SsoUser { get; set; }
	}
}
