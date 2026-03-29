namespace Domain
{
    public class String
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public Guid Code { get; set; }
        public bool DeleteAfterDownload { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
