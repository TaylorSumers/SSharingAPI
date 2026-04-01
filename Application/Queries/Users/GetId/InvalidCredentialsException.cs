namespace Application.Queries.Users.GetId
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base($"Invalid credentials.") { }
    }
}
