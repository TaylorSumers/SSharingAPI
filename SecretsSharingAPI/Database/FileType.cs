using System.Text.Json.Serialization;

namespace SecretsSharingAPI.Database
{
    public class FileType
    {
        public int ID { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<File> Files { get; set; }
    }
}
