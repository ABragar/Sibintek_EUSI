using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Base.Security;
using Base.Security.Service.Abstract;

namespace WebUI.Concrete
{
    public class PasswordService : IPasswordService
    {
        public string Generate(int length, PasswordCharacters allowedCharacters, char[] excludeCharacters)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length), @"Password length must be greater than zero");

            var randomBytes = new byte[length];
            var randomNumberGenerator = new RNGCryptoServiceProvider();
            randomNumberGenerator.GetBytes(randomBytes);

            var allowedCharactersString = GenerateAllowedCharactersString(allowedCharacters, excludeCharacters);
            var allowedCharactersCount = allowedCharactersString.Length;

            var characters = new char[length];
            for (var i = 0; i < length; i++)
                characters[i] = allowedCharactersString[randomBytes[i] % allowedCharactersCount];
            return new string(characters);
        }


        private static string GenerateAllowedCharactersString(PasswordCharacters characters, IEnumerable<char> excludeCharacters)
        {
            var allowedCharactersString = new StringBuilder();

            foreach (var type in AllowedPasswordCharacters.Where(type => (characters & type.Key) == type.Key))
            {
                if (excludeCharacters == null)
                    allowedCharactersString.Append(type.Value);
                else
                    allowedCharactersString.Append(type.Value.Where(c => !excludeCharacters.Contains(c)).ToArray());
            }

            return allowedCharactersString.ToString();
        }

        private static readonly Dictionary<PasswordCharacters, string> AllowedPasswordCharacters =
            new Dictionary<PasswordCharacters, string>(4) {
                { PasswordCharacters.LowercaseLetters, "abcdefghijklmnopqrstuvwxyz" },
                { PasswordCharacters.UppercaseLetters, "ABCDEFGHIJKLMNOPQRSTUVWXYZ" },
                { PasswordCharacters.Numbers, "0123456789" },
                { PasswordCharacters.Punctuations, @"~`!@#$%^&*()_-+={[}]|\:;""'<,>.?/" },
                { PasswordCharacters.Space, " " },
            };
    }
}