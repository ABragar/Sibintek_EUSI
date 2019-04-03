namespace Base.Security.Service.Abstract
{
    public interface IPasswordService
    {
        string Generate(int length, PasswordCharacters allowedCharacters = PasswordCharacters.All, char[] excludeCharacters = null);
    }
}