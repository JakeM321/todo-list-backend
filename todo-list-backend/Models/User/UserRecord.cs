using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todo_list_backend.Models.User
{
    public class UserRecord
    {
		public int Id { get; set; }
		public string Email { get; set; }
		public string DisplayName { get; set; }
		public string PasswordHash { get; set; }
		public string Salt { get; set; }
		public bool SsoUser { get; set; }
	}
}
