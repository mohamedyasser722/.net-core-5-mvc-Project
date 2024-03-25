using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
	public static class DocumentSettings
	{
		// Upload 
		

		public static string UploadFile(IFormFile file , string folderName)
		{
			// 1. Get Located Folder Path

			string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

			// 2. Get File Name and Make it Unique

			string FileName = $"{Guid.NewGuid().ToString() + file.FileName}";
			
			// 3. Get File Path[Folder Path + FileName]

			string FilePath = Path.Combine(FolderPath, FileName);

			// 4. Save File As Streams

			using (var stream = new FileStream(FilePath, FileMode.Create))
			{
				file.CopyTo(stream);
			}	

			// 5. Return File Name

			return FileName;
		}

		// Delete

		public static void DeleteFile(string fileName, string folderName)
		{
			// 1. Get Located Folder Path

			string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

			// 2. Get File Path[Folder Path + FileName]

			string FilePath = Path.Combine(FolderPath, fileName);

			// 3. Delete File

			if (File.Exists(FilePath))
			{
				File.Delete(FilePath);
			}
		}
	}
}
