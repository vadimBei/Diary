using AutoMapper;
using Diary.BLL.Models.Invite;
using Diary.BLL.Models.User;
using Diary.BLL.Services.AesCryptoProvider;
using Diary.BLL.Services.EmailSender;
using Diary.BLL.Services.InviteService;
using Diary.BLL.Services.UserService;
using Diary.DAL.Common;
using Diary.DAL.Entities;
using Diary.WEB.ViewModels.Invite;
using Diary.WEB.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diary.WEB.Controllers
{
	[Authorize(Roles = "admin")]
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly UserManager<User> _userManager;
		private readonly IUserService _userService;
		private readonly IInviteService _inviteService;
		private readonly IMapper _mapper;
		private readonly IEmailSenderService _emailSenderService;
		private readonly IAesCryptoProviderService _aesCryptoProvider;

		public AdminController(
			ApplicationDbContext dbContext,
			UserManager<User> userManager,
			IUserService userService,
			IInviteService inviteService,
			IAesCryptoProviderService aesCryptoProvider,
			IMapper mapper,
			IEmailSenderService emailSenderService)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_userService = userService;
			_inviteService = inviteService;
			_aesCryptoProvider = aesCryptoProvider;
			_mapper = mapper;
			_emailSenderService = emailSenderService;
		}

		public IActionResult Index()
		{
			var users = _userService.GetAll();

			var userViewModels = _mapper.Map<List<UserViewModel>>(users);

			return View(userViewModels);
		}

		[HttpGet]
		public IActionResult GetEmailForInvitation()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> GetEmailForInvitation(InviteViewModel inviteViewModel)
		{
			var checkedEmail = _dbContext.Users.FirstOrDefault(u => u.Email == inviteViewModel.EmailNewUser);

			if (checkedEmail != null)
			{
				return Content(Diary.Resource.Resource.GetEmailForInvitation_ExitingEmail);
			}

			var newInvite = _mapper.Map<InviteCreateModel>(inviteViewModel);
			_inviteService.Create(newInvite);

			var invite = _dbContext.Invites.FirstOrDefault(i => i.EmailNewUser == inviteViewModel.EmailNewUser);

			var callBack = Url.Action(
				"CheckInvite",
				"Account",
				new { token = invite.Id },
				protocol: HttpContext.Request.Scheme);

			await _emailSenderService.SendEmailAcync(inviteViewModel.EmailNewUser,
				"Запрошення для реєстрації на сайті",
				$"{inviteViewModel.Message} <a href='{callBack}'>Diary</a>");

			return RedirectToAction("Index", "Home");
		}


		[HttpGet]
		public async Task<IActionResult> CryptoKey()
		{
			var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

			var admin = _userService.Get(currentUser.Id);

			var userViewModel = new UserViewModel();

			if (admin.CryptoKey == null)
			{
				userViewModel.EmptyCryptoKey = true;
			}

			return View(userViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCryptokey()
		{
			var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

			var cryptoKey = _aesCryptoProvider.GenerateKey();

			var admin = _userService.Get(currentUser.Id);

			var updatedAdmin = _mapper.Map<UserUpdateModel>(new UserUpdateModel
			{
				Id = admin.Id,
				CryptoKey = cryptoKey,
				Email = admin.Email,
				DateCreation = admin.DateCreation,
				ModifiedDate = DateTime.Now,
				UserName = admin.UserName
				
			});

			_userService.Update(updatedAdmin);

			return RedirectToAction("Cryptokey", "Admin");
		}
	}
}