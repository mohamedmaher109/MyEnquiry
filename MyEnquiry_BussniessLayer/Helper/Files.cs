using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Helper
{
	public class Files
	{
		public static string SaveImage(IFormFile image, IWebHostEnvironment _environment , bool isApi = false)
		{
			if (!Directory.Exists(_environment.WebRootPath + "/uploads/images/"))
			{
				Directory.CreateDirectory(_environment.WebRootPath + "/uploads/images/");
			}
			string magePath = "/uploads/images/" + RandomHelper.GenerateUniqueID(25) + Path.GetExtension(image.FileName).ToLower();
			using (FileStream filestream = File.Create(_environment.WebRootPath + magePath))
			{
				image.CopyTo(filestream);
				filestream.Flush();
				var fileExtention = Path.GetExtension(image.FileName).ToLower();
			}
            if (isApi)
            {
                return $"https://api.est3lamy.com{magePath}" ;
            }
            else
            {
                return $"https://est3lamy.com{magePath}";
            }
            
		}
		public static string SaveWord(IFormFile image, IHostingEnvironment _environment)
		{
			if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\word\\"))
			{
				Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\word\\");
			}
			string magePath = "\\uploads\\word\\" + RandomHelper.GenerateUniqueID(25) + Path.GetExtension(image.FileName).ToLower();
			using (FileStream filestream = File.Create(_environment.WebRootPath + magePath))
			{
				image.CopyTo(filestream);
				filestream.Flush();
				var fileExtention = Path.GetExtension(image.FileName).ToLower();
			}
			return magePath;
		}	
		public static string SavePdf(IFormFile image, IHostingEnvironment _environment)
		{
			if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\pdf\\"))
			{
				Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\pdf\\");
			}
			string magePath = "\\uploads\\pdf\\" + RandomHelper.GenerateUniqueID(25) + Path.GetExtension(image.FileName).ToLower();
			using (FileStream filestream = File.Create(_environment.WebRootPath + magePath))
			{
				image.CopyTo(filestream);
				filestream.Flush();
				var fileExtention = Path.GetExtension(image.FileName).ToLower();
			}
			return magePath;
		}

		public static string SaveExcel(IFormFile image, IWebHostEnvironment _environment)
		{
			if (!Directory.Exists(_environment.WebRootPath + "\\uploads\\excel\\"))
			{
				Directory.CreateDirectory(_environment.WebRootPath + "\\uploads\\excel\\");
			}
			string magePath = "\\uploads\\excel\\" + RandomHelper.GenerateUniqueID(25) + Path.GetExtension(image.FileName).ToLower();
			using (FileStream filestream = File.Create(_environment.WebRootPath + magePath))
			{
				image.CopyTo(filestream);
				filestream.Flush();
				var fileExtention = Path.GetExtension(image.FileName).ToLower();
			}
			return magePath;
		}

		public static bool IsImage(IFormFile file)
		{
			if (file != null)
			{
				List<string> validImages = new List<string>
				{
					"tiff", "pjp", "pjpeg", "jfif", "tif", "svg", "bmp", "jpeg", "jpg", "png", "gif", "svgz", "webp", "ico", "xbm", "dib"
				};

				var extension = Path.GetExtension(file.FileName).Substring(1).ToLower();

				if (validImages.Contains(extension))
					return true;
			}

			return false;
		}

		public static bool IsExcel(IFormFile file)
		{
			if (file != null)
			{
				List<string> validExcels = new List<string>
				{
					"xlsx", "xls", "csv"
				};

				var extension = Path.GetExtension(file.FileName).Substring(1).ToLower();

				if (validExcels.Contains(extension))
					return true;
			}

			return false;
		}

		public static bool IsWord(IFormFile file)
		{
			if (file != null)
			{
				List<string> validWords = new List<string>
				{
					"doc","docx","rtf"
				};

				var extension = Path.GetExtension(file.FileName).Substring(1).ToLower();

				if (validWords.Contains(extension))
					return true;
			}

			return false;
		}

	}
}
