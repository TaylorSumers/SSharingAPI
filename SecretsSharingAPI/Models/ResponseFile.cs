namespace SecretsSharingAPI.Models
{
    public class ResponseFile
    {
        public ResponseFile(Database.File file) 
        {
            ID = file.ID;
            Name = file.Name;
            DeleteAfterDownload = file.DeleteAfterDownload ? "Да" : "Нет";
            FileType = file.FileType.Name;
            Code = file.Code;
            GetUrl = file.FileType.ID == 1 ? $"https://localhost:44306/api/Files/GetFile/code={file.Code}" : $"https://localhost:44306/api/Files/GetString/code={file.Code}";
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string DeleteAfterDownload { get; set; }

        public string FileType { get; set; }

        public string Code { get; set; }

        public string GetUrl { get; set; }
    }
}
