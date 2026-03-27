namespace Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<String> Strings { get; set; }
    }
}
