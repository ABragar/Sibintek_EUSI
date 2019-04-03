using System;

namespace Base.Security
{
    [Flags]
    public enum PasswordCharacters
    {
        LowercaseLetters = 0x01,
        UppercaseLetters = 0x02,
        Numbers = 0x04,
        Punctuations = 0x08,
        Space = 0x10,
        AllLetters = LowercaseLetters | UppercaseLetters,
        AlphaNumeric = AllLetters | Numbers,
        All = AllLetters | Numbers | Punctuations | Space,
    }
}