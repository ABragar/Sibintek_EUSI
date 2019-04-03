namespace CorpProp.Interfaces
{
    public interface IImportResultTextGenerator
    {
       string GetSuccessResultText(string mnemonic, string resultText, int count);
    }
}