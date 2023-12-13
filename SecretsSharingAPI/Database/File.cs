using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretsSharingAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json.Serialization;
using SecretsSharingAPI.Models;

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
