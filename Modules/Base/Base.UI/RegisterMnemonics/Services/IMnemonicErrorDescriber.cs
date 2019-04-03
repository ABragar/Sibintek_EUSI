using Base.UI.RegisterMnemonics.Entities;

namespace Base.UI.RegisterMnemonics.Services
{
    public interface IMnemonicErrorDescriber
    {
        string NotFound(string mnemonic);
        string Duplicate(string mnemonic);
        string DuplicateExtension<T>() where T : MnemonicEx;
    }

    public class MnemonicErrorDescriber : IMnemonicErrorDescriber
    {
        public string NotFound(string mnemonic)
        {
            return $"Мнемоника [{mnemonic}] не найдена";
        }

        public string Duplicate(string mnemonic)
        {
            return $"Мнемоника [{mnemonic}] уже зарегистрирована в системе";
        }

        public string DuplicateExtension<T>() where T : MnemonicEx
        {
            return $"Расширение [{typeof(T).Name}] для данной мнемоники уже добавлено";
        }
    }
}
