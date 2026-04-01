namespace Application.Commands.Users.Create
{
    public class CreateUserException : Exception
    {
        public CreateUserException(string login) : base($"User with login {login} already exists.") { }
    }
}
