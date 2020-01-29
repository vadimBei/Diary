using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Diary.WEB.Controllers
{
	public class UserController : Controller
	{
		public IActionResult SettingsByUser()
		{
			return View();
		}
	}
}