using Microsoft.AspNetCore.Mvc;

namespace Admin_Management.Data.Services
{
    public class MediaService
    {
        private readonly IWebHostEnvironment _environment;     
        public MediaService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string UploadSingle(IFormFile FileData, string folderPath)
        {
            folderPath += FileData.FileName;
            var uploadPath = _environment.WebRootPath + folderPath;
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                FileData.CopyTo(fileStream);
            }
            return folderPath;
        }

        public List<string> GetMediaFiles(string folderPath)
        {
            List<string> mediaFiles = new List<string>();
            string projectRootPath = _environment.WebRootPath;
            string newpath = projectRootPath + folderPath.Replace("/", "\\");
            if (Directory.Exists(newpath))
            {
                var rData = Directory.GetFiles(newpath).ToList();
                if (rData != null && rData.Count > 0)
                {
                    foreach (var item in rData)
                    {
                        mediaFiles.Add(item.Replace(projectRootPath, "").Replace("\\","/"));
                    }
                }
            }


            return mediaFiles;

        }
    }
}
