using SecretsSharingAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SecretsSharingAPI.Database
{
    public class User
    {
        public User() 
        {
            this.Files = new HashSet<File>();
        }

        public User(RequiredUser user)
        {
            Login = user.Login;
            Password = user.Password;
        }

        public int ID { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public virtual ICollection<File> Files { get; set; }
    }
}
