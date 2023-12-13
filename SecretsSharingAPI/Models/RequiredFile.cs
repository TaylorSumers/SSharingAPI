namespace SecretsSharingAPI.Models
{
    public class RequiredFile
    {
        public int Uploader { get; set; }

        public IFormFile FileToUpload { get; set; }

        public bool DeleteAfterDownload { get; set; }
    }
}
