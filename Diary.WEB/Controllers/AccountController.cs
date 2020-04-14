using Diary.BLL.Infrastructure;
using Diary.BLL.Models.Account;
using Diary.BLL.Services.AesCryptoProvider;
using Diary.BLL.Services.CaptchaService;
using Diary.BLL.Services.EmailSender;
using Diary.DAL.Common;
using Diary.DAL.Entities;
using Diary.WEB.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Diary.WEB.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly ICaptchaService _captchaService;
		private readonly IAesCryptoProviderService _aesCrypto;
		private readonly ApplicationDbContext _dbContext;
		private readonly IEmailSenderService _emailSender;

		public AccountController(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			ICaptchaService captchaService,
			IAesCryptoProviderService aesCrypto,
			ApplicationDbContext dbContext,
			IEmailSenderService emailSender)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_captchaService = captchaService;
			_aesCrypto = aesCrypto;
			_dbContext = dbContext;
			_emailSender = emailSender;
		}

		//return captcha image
		[Route("get-captcha-image")]
		public IActionResult GetCaptchaImage()
		{
			int width = 100;
			int height = 36;
			var captchaCode = _captchaService.GenerateCaptchaCode();
			var result = _captchaService.GenerateCaptchaImage(width, height, captchaCode);
			HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
			Stream s = new MemoryStream(result.CaptchaByteData);
			return new FileStreamResult(s, "image/png");
		}

		[HttpGet]
		public IActionResult Register()
		{
			if (HttpContext.User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterModel registerModel)
		{
			if (!_captchaService.ValidateCaptchaCode(registerModel.Captcha, HttpContext))
			{
				return View();
			}

			if (!ModelState.IsValid)
			{
				ModelState.AddModelError(string.Empty, Diary.Resource.Resource.ModelIsNotValid);

				return View(registerModel);
			}

			try
			{
				byte[] cryptokey = _aesCrypto.GenerateKey();

				var checkEmail = _dbContext.Users.FirstOrDefaultAsync(e => e.Email == registerModel.Email);
				var checkUserName = _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == registerModel.UserName);

				if (checkEmail != null)
				{
					ModelState.AddModelError(string.Empty, Diary.Resource.Resource.Register_ExitingEmail);
				}
				if (checkUserName != null)
				{
					ModelState.AddModelError(string.Empty, Diary.Resource.Resource.Register_ExitingUserName);
				}

				var user = new User
				{
					Email = registerModel.Email,
					UserName = registerModel.UserName,
					CryptoKey = cryptokey,
					DateCreation = DateTime.Now,
					ModifiedDate = DateTime.Now
				};

				var result = await _userManager.CreateAsync(user, registerModel.Password);

				if (!result.Succeeded)
				{
					ModelState.AddModelError(string.Empty, Diary.Resource.Resource.UserDidNotCreate);
				}

				var currentUser = await _userManager.FindByNameAsync(user.UserName);
				var roleResult = await _userManager.AddToRoleAsync(currentUser, "user");

				await _signInManager.SignInAsync(user, false);

				return RedirectToAction("Index", "Home");
			}
			catch (ValidationException ex)
			{
				ModelState.AddModelError(ex.Property, ex.Message);

				return View(registerModel);
			}
			catch (DbUpdateException ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);

				return View(registerModel);
			}
		}


		[HttpGet]
		public IActionResult Login(string returnUrl = null)
		{
			if (HttpContext.User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}

			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel viewModelmodel)
		{
			if (ModelState.IsValid)
			{
				// authentication of user
				var result = await _signInManager.PasswordSignInAsync(viewModelmodel.UserName, viewModelmodel.Password, viewModelmodel.RememberMe, false);

				if (result.Succeeded)
				{
					//check belong URL to application
					if (!string.IsNullOrEmpty(viewModelmodel.ReturnUrl) && Url.IsLocalUrl(viewModelmodel.ReturnUrl))
					{
						return Redirect(viewModelmodel.ReturnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", Diary.Resource.Resource.FailLoginOrPassword);
				}
			}
			else
			{
				ModelState.AddModelError(string.Empty, Diary.Resource.Resource.ModelIsNotValid);
			}

			return View(viewModelmodel);
		}

		[HttpGet]
		public async Task<IActionResult> LogOff()
		{
			// delete authentication cookies
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account");
		}

		[HttpGet]
		public IActionResult SendLinkForChengePassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendLinkForChengePassword(string email)
		{
			var searchUser = _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

			if (searchUser == null)
			{
				return Content(Diary.Resource.Resource.NonexistentEmail);
			}

			var callBack = Url.Action(
				"ChangePasswordWithoutOld",
				"Account",
				new { userEmail = email },
				protocol: HttpContext.Request.Scheme);

			await _emailSender.SendEmailAcync(email, "Відновлення паролю",
			 $"Для відновлення паролю, передіть за посиланням <a href='{callBack}'>Diary</a>");

			return RedirectToAction("Login", "Account");
		}

		[HttpGet]
		public async Task<IActionResult> ChangePasswordWithoutOld(string userEmail)
		{
			var user = await _userManager.FindByEmailAsync(userEmail);

			if (user == null)
			{
				return NotFound();
			}

			var forgotPasswordViewModel = new ForgotPasswordViewModel()
			{
				Email = user.Email,
				UserName = user.UserName
			};

			return View(forgotPasswordViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> ChangePasswordWithoutOld(ForgotPasswordViewModel forgotPasswordViewModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);

				if (user != null)
				{
					var _passwordValidator =
				HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
					var _passwordHasher =
						HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

					IdentityResult result =
						await _passwordValidator.ValidateAsync(_userManager, user, forgotPasswordViewModel.NewPassword);

					if (result.Succeeded)
					{
						user.PasswordHash = _passwordHasher.HashPassword(user, forgotPasswordViewModel.NewPassword);

						await _userManager.UpdateAsync(user);

						return RedirectToAction("Login", "Account");
					}
					else
					{
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, Diary.Resource.Resource.UserNotFound);
				}
			}
			else
			{
				ModelState.AddModelError(string.Empty, Diary.Resource.Resource.ModelIsNotValid);
			}

			return View(forgotPasswordViewModel);
		}

		[HttpGet]
		[Authorize(Roles = "admin, user")]
		public async Task<IActionResult> ChangePassword()
		{
			if (!HttpContext.User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Login", "Account");
			}

			var currentUser = HttpContext.User.Identity.Name;
			var user = await _userManager.FindByNameAsync(currentUser);

			if (user == null)
			{
				return NotFound();
			}

			var changePasswordViewModel = new ChangePasswordViewModel()
			{
				Email = user.Email,
				UserName = user.UserName
			};

			return View(changePasswordViewModel);
		}

		[HttpPost]
		[Authorize(Roles = "admin, user")]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(changePasswordViewModel.Email);

				if (user != null)
				{
					IdentityResult result =
						await _userManager.ChangePasswordAsync(user, changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword);

					if (result.Succeeded)
					{
						return RedirectToAction("Index", "Home");
					}
					else
					{
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, Diary.Resource.Resource.UserNotFound);
				}
			}
			else
			{
				ModelState.AddModelError(string.Empty, Diary.Resource.Resource.ModelIsNotValid);
			}
			return View(changePasswordViewModel);
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult CheckInvite(Guid token)
		{
			var tok = _dbContext.Invites.FirstOrDefault(i => i.Id == token);

			if (tok == null)
			{
				return Content(Diary.Resource.Resource.FailInvite);
			}

			return RedirectToAction("Register", "Account");
		}

		public IActionResult SettingsByUser()
		{
			return View();
		}
	}
}