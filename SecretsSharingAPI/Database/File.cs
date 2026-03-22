using System.Text.Json.Serialization;

namespace SecretsSharingAPI.Database
{
    public class File
    {
        public int ID {  get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool DeleteAfterDownload { get; set; }
        public virtual FileType FileType { get; set; }
        [JsonIgnore]
        public virtual User Uploader { get; set; }
    }
}
