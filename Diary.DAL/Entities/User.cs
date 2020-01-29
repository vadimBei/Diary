using Diary.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Diary.DAL.Entities
{
	public class User : IdentityUser<Guid>, IBaseEntity<Guid>
	{
		public List<Record> Records { get; set; }

		DateTime? DeletedDate { get; set; }

		public byte[] CryptoKey { get; set; }

		public DateTime DateCreation { get; set; }

		public DateTime ModifiedDate { get; set; }
	}
}
