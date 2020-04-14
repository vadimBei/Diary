using AutoMapper;
using Diary.BLL.Models.Record;
using Diary.BLL.Models.UploadedFile;
using Diary.BLL.Services.AesCryptoProvider;
using Diary.BLL.Services.RecordService;
using Diary.BLL.Services.UploadedFileServise;
using Diary.DAL.Common;
using Diary.DAL.Entities;
using Diary.WEB.ViewModels.Record;
using Diary.WEB.ViewModels.UploadedFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Diary.WEB.Controllers
{
	[Authorize(Roles = "admin, user")]
	public class RecordController : Controller
	{
		private readonly IAesCryptoProviderService _aesCryptoProvider;
		private readonly IUploadedFileService _uploadedFileService;
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly ApplicationDbContext _dbContext;
		private readonly UserManager<User> _userManager;
		private readonly IRecordService _recordService;
		private readonly IMapper _mapper;

		public RecordController(
			IAesCryptoProviderService aesCryptoProvider,
			IUploadedFileService uploadedFileService,
			IHostingEnvironment hostingEnvironment,
			ApplicationDbContext dbContext,
			UserManager<User> userManager,
			IRecordService recordService,
			IMapper mapper
			)
		{
			_uploadedFileService = uploadedFileService;
			_hostingEnvironment = hostingEnvironment;
			_aesCryptoProvider = aesCryptoProvider;
			_recordService = recordService;
			_userManager = userManager;
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public IActionResult CreateRecord()
		{
			return View();
		}

		[HttpPost]
		[DisableRequestSizeLimit]
		public async Task<IActionResult> CreateRecord(RecordViewModel recordViewModel)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError(string.Empty, Diary.Resource.Resource.DBError);

				return View(recordViewModel);
			}

			try
			{
				var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
				var ivKey = _aesCryptoProvider.GenerateIV();

				if (recordViewModel.UploadedFiles != null)
				{
					var createModel = _mapper.Map<RecordCreateModel>(new RecordCreateModel
					{
						Name = recordViewModel.Name,
						DateCreation = DateTime.Now,
						ModifiedDate = DateTime.Now,
						IvKey = ivKey,
						Text = _aesCryptoProvider.EncryptValue(recordViewModel.Text, currentUser.CryptoKey, ivKey),
						UserId = currentUser.Id
					});

					var recordId = _recordService.Create(createModel);

					//method for upload files
					UploadFile(recordId, recordViewModel.UploadedFiles);
				}
				else
				{
					var createModel = _mapper.Map<RecordCreateModel>(new RecordCreateModel
					{
						Name = recordViewModel.Name,
						DateCreation = DateTime.Now,
						ModifiedDate = DateTime.Now,
						IvKey = ivKey,
						Text = _aesCryptoProvider.EncryptValue(recordViewModel.Text, currentUser.CryptoKey, ivKey),
						UserId = currentUser.Id
					});

					_recordService.Create(createModel);
				}

				return RedirectToAction("Index", "Home");
			}
			catch (DbUpdateException ex)
			{
				ModelState.AddModelError("", ex.ToString());
			}

			return View(recordViewModel);
		}

		[HttpGet]
		public async Task<IActionResult> EditRecord(Guid id)
		{
			if (id == null)
			{
				return NotFound();

			}

			var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

			var record = _recordService.Get(id);

			var filesToView = _uploadedFileService.GetAll().Where(f => f.RecordId == record.Id);

			var filesViewModel = new List<UploadedFileViewModel>();

			if (filesToView != null)
			{
				foreach (var file in filesToView)
				{
					var fileToView = _mapper.Map<UploadedFileViewModel>(new UploadedFileViewModel
					{
						Name = file.Name,
						Path = file.Path,
						DateCreation = file.DateCreation,
						ModifiedDate = file.ModifiedDate,
						RecordId = file.RecordId,
						IsImage = file.IsImage
					});

					filesViewModel.Add(fileToView);
				}
			}

			var recordToView = _mapper.Map<RecordViewModel>(new RecordViewModel
			{
				Id = record.Id,
				Name = record.Name,
				DateCreation = record.DateCreation,
				ModifiedDate = record.ModifiedDate,
				Text = _aesCryptoProvider.DecryptValue(record.Text, currentUser.CryptoKey, record.IvKey),
				UserId = record.UserId,
				UploadedFileViewModels = filesViewModel
			});

			return View(recordToView);
		}

		[HttpPost]
		[DisableRequestSizeLimit]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditRecord(RecordViewModel recordViewModel)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError(string.Empty, Diary.Resource.Resource.ModelIsNotValid);

				return View(recordViewModel);
			}

			try
			{
				var record = _recordService.Get(recordViewModel.Id);

				var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

				if (recordViewModel.UploadedFiles != null)
				{
					//method for upload files
					UploadFile(record.Id, recordViewModel.UploadedFiles);
				}

				var recordToUpdate = _mapper.Map<RecordUpdateModel>(new RecordUpdateModel
				{
					Id = recordViewModel.Id,
					ModifiedDate = DateTime.Now,
					Name = recordViewModel.Name,
					DateCreation = record.DateCreation,
					Text = _aesCryptoProvider.EncryptValue(recordViewModel.Text, currentUser.CryptoKey, record.IvKey),
					UserId = recordViewModel.UserId,
					IvKey = record.IvKey,
				});

				_recordService.Update(recordToUpdate);

				return RedirectToAction("Index", "Home");
			}
			catch (DbUpdateException ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
			}

			return View(recordViewModel);
		}

		[HttpGet]
		public async Task<IActionResult> ViewRecord(Guid id)
		{
			if (id == null)
				return NotFound();

			var exitingRecord = _recordService.Get(id);

			var currentUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

			var exitingFiles = _uploadedFileService.GetAll().Where(f => f.RecordId == exitingRecord.Id);

			var filesViewModel = new List<UploadedFileViewModel>();

			if (exitingFiles != null)
			{
				foreach (var file in exitingFiles)
				{
					var fileToView = _mapper.Map<UploadedFileViewModel>(new UploadedFileViewModel
					{
						Name = file.Name,
						Path = file.Path,
						DateCreation = file.DateCreation,
						ModifiedDate = file.ModifiedDate,
						RecordId = file.RecordId,
						IsImage = file.IsImage
					});

					filesViewModel.Add(fileToView);
				}
			}

			var recordToView = _mapper.Map<RecordViewModel>(new RecordViewModel
			{
				Id = exitingRecord.Id,
				Name = exitingRecord.Name,
				DateCreation = exitingRecord.DateCreation,
				ModifiedDate = exitingRecord.ModifiedDate,
				Text = _aesCryptoProvider.DecryptValue(exitingRecord.Text, currentUser.CryptoKey, exitingRecord.IvKey),
				UploadedFileViewModels = filesViewModel
			});

			return View(recordToView);
		}

		public void UploadFile(Guid recordId, List<IFormFile> formFiles)
		{
			foreach (var formFile in formFiles)
			{
				var formType = formFile.ContentType;

				var uploadedFileViewModel = new UploadedFileViewModel();

				if (GetMimeTypeOfPhotos().ContainsValue(formType))
				{
					//path for save images
					var pathForImage = Path.Combine(_hostingEnvironment.WebRootPath, "images", formFile.FileName);

					using (Stream stream = formFile.OpenReadStream())
					{
						using (Image image = Image.Load(stream))
						{
							image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
							image.Save(pathForImage);
						}
					}

					uploadedFileViewModel.IsImage = true;
					uploadedFileViewModel.Path = pathForImage;
				}
				else
				{
					//path for save files
					var pathForFile = Path.Combine(_hostingEnvironment.WebRootPath, "Files", formFile.FileName);

					using (FileStream fileStream = new FileStream(pathForFile, FileMode.Create))
					{
						formFile.CopyTo(fileStream);
					}

					uploadedFileViewModel.Path = pathForFile;
				}

				var newFile = _mapper.Map<UploadedFileCreateModel>(new UploadedFileCreateModel
				{
					Name = formFile.FileName,
					Path = uploadedFileViewModel.Path,
					DateCreation = DateTime.Now,
					ModifiedDate = DateTime.Now,
					RecordId = recordId,
					IsImage = uploadedFileViewModel.IsImage
				});

				_uploadedFileService.Create(newFile);
			}
		}

		public async Task<IActionResult> Download(string fileName)
		{
			if (fileName == null)
				return Content("Ім'я файлу відсутнє");

			var exitingItem = await _dbContext.UploadedFiles.FirstOrDefaultAsync(u => u.Name == fileName);

			var memory = new MemoryStream();

			using (var stream = new FileStream(exitingItem.Path, FileMode.Open))
			{
				await stream.CopyToAsync(memory);
			}

			memory.Position = 0;

			return File(memory, GetContentType(exitingItem.Path), Path.GetFileName(exitingItem.Path));
		}

		private string GetContentType(string path)
		{
			var types = GetMimeTypes();

			var ext = Path.GetExtension(path).ToLowerInvariant();

			return types[ext];
		}

		private Dictionary<string, string> GetMimeTypes()
		{
			return new Dictionary<string, string>
			{
				{".txt", "text/plain"},
				{".pdf", "application/pdf"},
				{".doc", "application/vnd.ms-word"},
				{".docx", "application/vnd.ms-word"},
				{".xls", "application/vnd.ms-excel"},
				{".png", "image/png"},
				{".jpg", "image/jpeg"},
				{".jpeg", "image/jpeg"},
				{".gif", "image/gif"},
				{".csv", "text/csv"}
			};
		}

		private Dictionary<string, string> GetMimeTypeOfPhotos()
		{
			return new Dictionary<string, string>
			{
				{".png", "image/png"},
				{".jpg", "image/jpeg"},
				{".jpeg", "image/jpeg"},
				{".gif", "image/gif"},
			};
		}
	}
}