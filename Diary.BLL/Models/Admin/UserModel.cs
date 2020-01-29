using System;
using System.Collections.Generic;
using System.Text;

namespace Diary.BLL.Models.Admin
{
	public class UserModel : BaseModel
	{
		public string UserName { get; set; }

		public string Email { get; set; }
	}
}
