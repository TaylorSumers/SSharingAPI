namespace SecretsSharingAPI.Models
{
    public class RequiredString
    {
        public string Content { get; set; }

        public int Uploader { get; set; }

        public bool DeleteAfterDownload { get; set; }
    }
}
