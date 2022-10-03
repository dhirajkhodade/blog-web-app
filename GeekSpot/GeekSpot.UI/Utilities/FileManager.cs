namespace GeekSpot.UI.Utilities
{
    public class FileManager
    {
        private IWebHostEnvironment _environment;
        public FileManager(IWebHostEnvironment Environment)
        {
            _environment = Environment;
        }
        public string SaveImageToDisk(IFormFile file)
        {
            var uniqueFileName = "";
            var fullFilePath = "";
            if (file != null)
            {
                var uploadfilepath = $"{_environment.WebRootPath}\\assets\\images";
                uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                fullFilePath = Path.Combine(uploadfilepath, uniqueFileName);
                file.CopyTo(new FileStream(fullFilePath, FileMode.Create));
            }
            return $"/assets/images/{uniqueFileName}";
        }
    }
}
